using Bazaar.Entityframework.Models;
using Bazaar.Entityframework.Services;
using Bazaar.Entityframework.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bazaar.app.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient _bot;
        private readonly IUserStateService _userStateService;
        private readonly IPackageDataService _packageService;
        private readonly IPaymentGatewayDataService _gatewayService;
        private readonly IPaymentRequestService _paymentRequestService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        public TelegramBotService(IUserStateService userStateService, IPackageDataService packageService, IPaymentGatewayDataService gatewayService, IPaymentRequestService paymentRequestService, UserManager<AppUser> userManager, IWebHostEnvironment env, IConfiguration config)
        {
            _config = config;

            string botToken = _config.GetValue<string>("Telegram:Token")!;
            _bot = new TelegramBotClient(botToken);
            _userStateService = userStateService;
            _packageService = packageService;
            _gatewayService = gatewayService;
            _paymentRequestService = paymentRequestService;
            _userManager = userManager;
            _env = env;
        }
        public async Task HandleUpdate(Update update)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallback(update.CallbackQuery!);
                return;
            }

            if (update.Type == UpdateType.Message)
            {
                var msg = update.Message!;
                var userState = await _userStateService.GetOrCreateAsync(msg.Chat.Id);

                if (msg.Text == "/start")
                {
                    await _userStateService.ResetStateAsync(msg.Chat.Id);
                    await _bot.SendMessage(msg.Chat.Id, "أهلاً بك في بوت دفع سياراتي 🚗\nالرجاء تزويدنا ببريدك الإلكتروني المسجل في التطبيق:");
                    userState.Step = BotStep.WaitingEmail;
                    await _userStateService.UpdateStateAsync(userState);
                    return;
                }

                switch (userState.Step)
                {
                    case BotStep.WaitingEmail:
                        await HandleEmailStep(userState, msg);
                        break;

                    case BotStep.WaitingReceiptImage:
                        if (msg.Photo != null)
                            await HandlePhotoStep(userState, msg);
                        else
                            await _bot.SendMessage(msg.Chat.Id, "⚠️ من فضلك أرسل صورة الوصل حصراً.");
                        break;
                }
            }
        }
        private async Task HandleEmailStep(TelegramUserState userState, Message msg)
        {
            var email = msg.Text?.Trim();
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                await _bot.SendMessage(msg.Chat.Id, "📧 يرجى إدخال بريد إلكتروني صحيح:");
                return;
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                await _bot.SendMessage(msg.Chat.Id, "❌ هذا البريد غير مسجل لدينا، تأكد من الحساب الصحيح:");
                return;
            }
            userState.Email = email;
            var packages = await _packageService.GetAllAsync();
            if (packages.Count() == 0)
            {
                await _bot.SendMessage(msg.Chat.Id, "لا يوجد باقات متاحة حاليا");
            }
            var buttons = packages.Select(p => new[]
            {
        InlineKeyboardButton.WithCallbackData($"{p.Name} - {p.Price}$", $"pkg_{p.Id}")
    }).ToArray();

            await _bot.SendMessage(msg.Chat.Id, $"✅ أهلاً {user.UserName}، اختر الباقة المطلوبة:",
                replyMarkup: new InlineKeyboardMarkup(buttons));

            userState.Step = BotStep.ChoosingPackage;
            await _userStateService.UpdateStateAsync(userState);
        }
        private async Task HandleCallback(CallbackQuery query)
        {
            var chatId = query.Message!.Chat.Id;
            var userState = await _userStateService.GetOrCreateAsync(chatId);
            var data = query.Data ?? "";

            if (data.StartsWith("pkg_"))
            {
                int pkgId = int.Parse(data.Replace("pkg_", ""));
                userState.SelectedPackageId = pkgId;
                userState.Step = BotStep.ChoosingGateway;
                await _userStateService.UpdateStateAsync(userState);

                var gateways = await _gatewayService.GetAllAsync();
                if (gateways.Count() == 0)
                {
                    await _bot.SendMessage(chatId, "لا يوجد بوابات متاحة حاليا");
                }
                var buttons = gateways.Where(g => g.IsActive).Select(g => new[]
                {
            InlineKeyboardButton.WithCallbackData(g.Name, $"gw_{g.Id}")
        }).ToArray();

                await _bot.EditMessageText(chatId, query.Message.MessageId, "💳 اختر طريقة الدفع المفضلة لديك:",
                    replyMarkup: new InlineKeyboardMarkup(buttons));
            }
            else if (data.StartsWith("gw_"))
            {
                int gwId = int.Parse(data.Replace("gw_", ""));
                var gateway = await _gatewayService.GetByIdAsync(gwId);

                userState.SelectedGatewayId = gwId;
                userState.Step = BotStep.WaitingReceiptImage;
                await _userStateService.UpdateStateAsync(userState);

                string instruction = $"💎 *{gateway.Name}*\n\n" +
                                     $"📍 معلومات الحساب:\n`{gateway.AccountNumber}`\n\n" +
                                     $"📝 تعليمات:\n{gateway.Instructions}\n\n" +
                                     "📸 يرجى إرسال *صورة الوصل* الآن:";

                await _bot.SendMessage(chatId, instruction, parseMode: ParseMode.Markdown);
            }
            else if (data == "confirm_all")
            {
                await _bot.EditMessageText(chatId, query.Message.MessageId, "⏳ جاري معالجة طلبك، يرجى الانتظار...");

                //try
                //{
                    string localPath = await SaveReceiptImage(userState.TempReceiptFileId!);

                    var package = await _packageService.GetByIdAsync(userState.SelectedPackageId!.Value);
                    var gateway = await _gatewayService.GetByIdAsync(userState.SelectedGatewayId!.Value);
                    var appUser = await _userManager.FindByEmailAsync(userState.Email!);

                    var request = new PaymentRequest
                    {
                        UserId = appUser!.Id,
                        UserEmail = userState.Email!,
                        PackageId = package.Id,
                        PackageName = package.Name,
                        PackagePrice = package.Price,
                        PaymentGatewayId = gateway.Id,
                        PaymentGatewayName = gateway.Name,
                        ReceiptImagePath = localPath,
                        Status = PaymentStatus.Pending,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _paymentRequestService.CreateRequestAsync(request);

                    await _userStateService.ResetStateAsync(chatId);
                    await _bot.SendMessage(chatId, "✅ تم إرسال طلبك بنجاح! سيتم مراجعته من قبل الإدارة وتفعيل باقتك قريباً.");
                //}
                //catch (Exception)
                //{
                //    await _bot.SendMessage(chatId, "❌ حدث خطأ أثناء حفظ الطلب، يرجى المحاولة مرة أخرى.");
                //}
            }
            else if (data == "reset")
            {
                await _userStateService.ResetStateAsync(chatId);
                await _bot.SendMessage(chatId, "تم إلغاء العملية. يمكنك البدء مجدداً عبر إرسال /start");
            }
        }
        private async Task HandlePhotoStep(TelegramUserState userState, Message msg)
        {
            var fileId = msg.Photo!.Last().FileId;
            userState.TempReceiptFileId = fileId;
            userState.Step = BotStep.WaitingConfirmation;
            await _userStateService.UpdateStateAsync(userState);

            var buttons = new[]
            {
        new[] { InlineKeyboardButton.WithCallbackData("✅ تأكيد وإرسال الطلب", "confirm_all") },
        new[] { InlineKeyboardButton.WithCallbackData("❌ إلغاء", "reset") }
    };

            await _bot.SendMessage(msg.Chat.Id, "وصلت الصورة! هل تود تأكيد طلب الاشتراك؟",
                replyMarkup: new InlineKeyboardMarkup(buttons));
        }
        private async Task<string> SaveReceiptImage(string fileId)
        {
            var file = await _bot.GetFile(fileId);

            string folderName = Path.Combine(_env.WebRootPath, "telegram");
            if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);
            string fileName = $"{Guid.NewGuid()}.jpg";
            string fullPath = Path.Combine(folderName, fileName);
            using (var saveFileStream = File.OpenWrite(fullPath))
            {
                await _bot.DownloadFile(file.FilePath!, saveFileStream);
            }
            return $"/telegram/{fileName}";
        }
    }
}
