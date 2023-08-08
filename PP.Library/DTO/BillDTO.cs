using PP.Library.Models;

namespace PP.Library.DTO
{
	public class BillDTO
	{
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DueDate { get; set; }
        public int ProjectId { get; set; }

        public BillDTO()
		{
		}

        public BillDTO(Bill b)
        {
            this.Id = b.Id;
            this.TotalAmount = b.TotalAmount;
            this.DueDate = b.DueDate;
            this.ProjectId = b.ProjectId;         
        }
    }
}

