using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaiTapLon.Models
{
    public class BookAction
    {
        [Key]
        public int BookActionId { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser? User { get; set; }
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Books? Book { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string ActionStatus { get; set; } = "Đang xử lý";
        [Required]
        [Display(Name = "Ngày mượn")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BorrowDate { get; set; } = DateTime.Now;
        [Display(Name = "Ngày trả dự kiến")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ReturnDateExpected { get; set; }
        [Display(Name = "Ngày trả")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ReturnDate { get; set; } = DateTime.Now;
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }
    }
}
