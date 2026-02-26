using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework.Models
{
    public class TelegramUserState
    {
        public int Id { get;set; }
        public long ChatId { get; set; }
        public BotStep Step { get; set; } = BotStep.Start;
        public string? Email { get; set; }
        public int? SelectedPackageId { get; set; }
        public int? SelectedGatewayId { get; set; }
        public string? TempReceiptFileId { get; set; }
        public DateTime LastInteraction { get; set; } = DateTime.UtcNow;
    }
}
