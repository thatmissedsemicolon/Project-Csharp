using PP.Library.DTO;

namespace PP.Library.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DueDate { get; set; }
        public int ProjectId { get; set; }

        public Bill(BillDTO dto)
        {
            this.Id = dto.Id;
            this.TotalAmount = dto.TotalAmount;
            this.DueDate = dto.DueDate;
            this.ProjectId = dto.ProjectId;
        }
    }
}
