using Newtonsoft.Json;
using System.IO;
using PP.Library.DTO;

namespace PP.API.Database
{
    public class Filebase
    {
        private string _root;
        private string _clientRoot;
        private string _employeeRoot;
        private string _timeRoot;
        private string _billRoot;
        private static Filebase? _instance;


        public static Filebase Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = @"/PracticePanther/PP.API"; // needs to be changed to current directory of your PC.
            _clientRoot = $"{_root}\\Clients";
            _employeeRoot = $"{_root}\\Employees";
            _timeRoot = $"{_root}\\TimeRecords";
            _billRoot = $"{_root}\\Bills";

            if (!Directory.Exists(_clientRoot) || !Directory.Exists(_employeeRoot) || !Directory.Exists(_timeRoot) || !Directory.Exists(_billRoot))
            {
                Directory.CreateDirectory(_clientRoot);
                Directory.CreateDirectory(_employeeRoot);
                Directory.CreateDirectory(_timeRoot);
                Directory.CreateDirectory(_billRoot);
            }
        }


        // Client & Project is inside the client so no need of project root

        private int LastClientId => Clients.Any() ? Clients.Select(c => c.Id).Max() : 0;
  
        public ClientDTO AddClient(ClientDTO c)
        {
            if(c.Id <= 0)
            {
                c.Id = LastClientId + 1;
            }

            var path = Path.Combine(_clientRoot, $"{c.Id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(c));

            return c;
        }

        public ClientDTO UpdateClient(ClientDTO c)
        { 
            var path = Path.Combine(_clientRoot, $"{c.Id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(c));

            return c;
        }

        public List<ClientDTO> Clients
        {
            get
            {
                var root = new DirectoryInfo(_clientRoot);
                var _clients = new List<ClientDTO>();
                foreach (var todoFile in root.GetFiles())
                {
                    var todo = JsonConvert.
                        DeserializeObject<ClientDTO>
                        (File.ReadAllText(todoFile.FullName));
                    if(todo != null)
                    {
                        _clients.Add(todo);
                    }
                }
                return _clients;
            }
        }

        public bool DeleteClient(string id)
        {
            var path = Path.Combine(_clientRoot, $"{id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return true;
        }

        public List<ClientDTO> GetAllClients()
        {
            return Clients;
        }

        // Employee

        private int LastEmployeeId => Employees.Any() ? Employees.Select(e => e.Id).Max() : 0;

        public EmployeeDTO AddEmployee(EmployeeDTO e)
        {
            if (e.Id <= 0)
            {
                e.Id = LastEmployeeId + 1;
            }

            var path = Path.Combine(_employeeRoot, $"{e.Id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(e));

            return e;
        }

        public EmployeeDTO UpdateEmployee(EmployeeDTO e)
        {
            var path = Path.Combine(_employeeRoot, $"{e.Id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(e));

            return e;
        }

        public List<EmployeeDTO> Employees
        {
            get
            {
                var root = new DirectoryInfo(_employeeRoot);
                var _employees = new List<EmployeeDTO>();
                foreach (var todoFile in root.GetFiles())
                {
                    var todo = JsonConvert.
                        DeserializeObject<EmployeeDTO>
                        (File.ReadAllText(todoFile.FullName));
                    if (todo != null)
                    {
                        _employees.Add(todo);
                    }
                }
                return _employees;
            }
        }

        public bool DeleteEmployee(string id)
        {
            var path = Path.Combine(_employeeRoot, $"{id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return true;
        }

        public List<EmployeeDTO> GetAllEmployees()
        {
            return Employees;
        }

        // Time

        private int LastTimeId => Times.Any() ? Times.Select(t => t.Id).Max() : 0;

        public TimeDTO AddTime(TimeDTO t)
        {
            if (t.Id <= 0)
            {
                t.Id = LastTimeId + 1;
            }

            var path = Path.Combine(_timeRoot, $"{t.Id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(t));

            return t;
        }

        public TimeDTO UpdateTime(TimeDTO t)
        {
            var path = Path.Combine(_timeRoot, $"{t.Id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(t));

            return t;
        }

        public List<TimeDTO> Times
        {
            get
            {
                var root = new DirectoryInfo(_timeRoot);
                var _times = new List<TimeDTO>();
                foreach (var timeFile in root.GetFiles())
                {
                    var time = JsonConvert.
                        DeserializeObject<TimeDTO>
                        (File.ReadAllText(timeFile.FullName));
                    if (time != null)
                    {
                        _times.Add(time);
                    }
                }
                return _times;
            }
        }

        public bool DeleteTime(string id)
        {
            var path = Path.Combine(_timeRoot, $"{id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return true;
        }

        public List<TimeDTO> GetAllTimes()
        {
            return Times;
        }

        // Bills

        private int LastBillId => Bills.Any() ? Bills.Select(b => b.Id).Max() : 0;

        public BillDTO AddBill(BillDTO b)
        {
            if (b.Id <= 0)
            {
                b.Id = LastBillId + 1;
            }

            var path = Path.Combine(_billRoot, $"{b.Id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(b));

            return b;
        }

        public BillDTO UpdateBill(BillDTO b)
        {
            var path = Path.Combine(_billRoot, $"{b.Id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(b));

            return b;
        }

        public List<BillDTO> Bills
        {
            get
            {
                var root = new DirectoryInfo(_billRoot);
                var _bills = new List<BillDTO>();
                foreach (var billFile in root.GetFiles())
                {
                    var bill = JsonConvert.
                        DeserializeObject<BillDTO>
                        (File.ReadAllText(billFile.FullName));
                    if (bill != null)
                    {
                        _bills.Add(bill);
                    }
                }
                return _bills;
            }
        }

        public bool DeleteBill(string id)
        {
            var path = Path.Combine(_billRoot, $"{id}.json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return true;
        }

        public List<BillDTO> GetAllBills()
        {
            return Bills;
        }
    }
}
