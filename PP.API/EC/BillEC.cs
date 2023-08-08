using PP.API.Database;
using PP.Library.DTO;

namespace PP.API.EC
{
    public class BillEC
    {
        private readonly Filebase _filebase;

        public BillEC()
        {
            _filebase = Filebase.Current;
        }

        public BillDTO Add(BillDTO bill)
        {
            var existingClient = _filebase.Clients.FirstOrDefault(c => c.Projects.Any(p => p.Id == bill.ProjectId));

            if (existingClient == null)
            {
                throw new Exception($"Client with a project of ID {bill.ProjectId} was not found");
            }

            var existingProject = existingClient.Projects.FirstOrDefault(p => p.Id == bill.ProjectId);

            var addedBill = _filebase.AddBill(bill);

            if (addedBill != null)
            {
                existingProject.Bills.Add(addedBill);
                _filebase.UpdateClient(existingClient);
            }

            return addedBill;
        }

        public IEnumerable<BillDTO> GetAllBills()
        {
            return _filebase.GetAllBills();
        }

        public BillDTO Update(BillDTO billToUpdate)
        {
            var existingBill = _filebase.Bills.FirstOrDefault(b => b.Id == billToUpdate.Id);

            if (existingBill == null)
            {
                throw new Exception($"Bill with ID {billToUpdate.Id} was not found");
            }

            existingBill.DueDate = billToUpdate.DueDate;
            existingBill.TotalAmount = billToUpdate.TotalAmount;

            _filebase.UpdateBill(existingBill);

            var clientWithProject = _filebase.Clients.FirstOrDefault(c => c.Projects.Any(p => p.Id == billToUpdate.ProjectId));
            if (clientWithProject == null)
            {
                throw new Exception($"Client with a project of ID {billToUpdate.ProjectId} was not found");
            }

            var project = clientWithProject.Projects.FirstOrDefault(p => p.Id == billToUpdate.ProjectId);

            var billInProject = project.Bills.FirstOrDefault(b => b.Id == existingBill.Id);
            if (billInProject != null)
            {
                billInProject.DueDate = existingBill.DueDate;
                billInProject.TotalAmount = existingBill.TotalAmount;
            }

            _filebase.UpdateClient(clientWithProject);

            return existingBill;
        }

        public bool DeleteById(string id)
        {
            var bill = _filebase.Bills.FirstOrDefault(b => b.Id == Convert.ToInt32(id));

            if (bill != null)
            {
                var clientWithProject = _filebase.Clients.FirstOrDefault(c => c.Projects.Any(p => p.Id == bill.ProjectId));
                if (clientWithProject == null)
                {
                    throw new Exception($"Client with a project of ID {bill.ProjectId} was not found");
                }

                var project = clientWithProject.Projects.FirstOrDefault(p => p.Id == bill.ProjectId);

                var billInProject = project.Bills.FirstOrDefault(b => b.Id == bill.Id);
                if (billInProject != null)
                {
                    project.Bills.Remove(billInProject);
                }

                _filebase.UpdateClient(clientWithProject);

                if (_filebase.DeleteBill(id))
                {
                    return true;
                }
                else
                {
                    throw new Exception("Failed to a delete bill from the server");
                }
            }

            throw new Exception("Bill not found");
        }

        public IEnumerable<BillDTO> GetById(int id)
        {
            var bills = _filebase.Bills.Where(b => b.Id == id);

            return bills;
        }

        public IEnumerable<BillDTO> Search(string query)
        {
            return _filebase.Bills.Where(t => t.DueDate.ToString().ToUpper().Contains(query.ToUpper())).Take(1000);
        }
    }
}
