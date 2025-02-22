﻿using System.ComponentModel.DataAnnotations;

namespace BaiTapLon.Models
{
    public class Books
    {
        [Key]
        [Display(Name = "Mã sách")]
        public int BookId { get; set; }
        [Display(Name = "Tên Sách")]
        public string BookName { get; set; }
        [Display(Name = "Tác giả")]
        public string Author { get; set; }
        [Display(Name = "Nhà xuất bản")]
        public string Publisher { get; set; }
        [Display(Name = "Mã thể loại")]
        public int BookTypeId { get; set; }
        [Display(Name = "Số lượng")]
        public int? Quantity { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }
        [Display(Name = "Thể loại")]
        public BookType? BookType { get; set; }
    }
}