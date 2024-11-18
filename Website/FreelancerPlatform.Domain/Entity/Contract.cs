using FreelancerPlatform.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("hop_dong")]
    public class Contract : EntityBase
    {
        [Column("ma_hop_dong")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("nguoi_tao")]
        [Required]
        public int CreateUser { get; set; }

        [Column("ten_hop_dong")]
        [Required]
        public string Name { get; set; }

        [Column("noi_dung_hop_dong")]
        [Required]
        public string Content { get; set; }

        [Column("doi_tac")]
        [Required]
        public int Partner {  get; set; }

        [Column("trang_thai_chap_nhan")]
        [Required]
        public int AcceptStatus { get; set; } = 0;
        [Column("trang_thai_hop_dong")]
        [Required]
        public bool ContractStatus { get; set; } = true;

        [Column("diem_doi_tac")]
        public int? PartnerPoints { get; set; }

        [Column("danh_gia_doi_tac")]
        public string? PartnerReview { get; set; }

        public int? ProjectId { get; set; }

        public List<Transaction> Transactions { get; set; }

    }
}
