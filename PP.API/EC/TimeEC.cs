using PP.API.Database;
using PP.Library.DTO;

namespace PP.API.EC
{
    public class TimeEC
    {
        private readonly Filebase _filebase;

        public TimeEC()
        {
            _filebase = Filebase.Current;
        }

        public TimeDTO Add(TimeDTO time)
        {
            var existingTime = _filebase.Times.FirstOrDefault(t => t.Id == time.Id);

            if (existingTime != null)
            {
                var addTime = _filebase.AddTime(time);

                return addTime;
            }

            var addedTime = _filebase.AddTime(time);

            if (addedTime != null)
            {
                var employee = _filebase.Employees.FirstOrDefault(e => e.Id == time.EmployeeId);

                if (employee != null)
                {
                    employee.TimeRecords.Add(addedTime);
                    _filebase.UpdateEmployee(employee);
                }
                else
                {
                    throw new Exception($"Employee with ID {time.EmployeeId} was not found");
                }
            }

            return addedTime;
        }

        public IEnumerable<TimeDTO> GetAllTimes()
        {
            return _filebase.GetAllTimes();
        }

        public TimeDTO Update(TimeDTO timeToUpdate)
        {
            var existingTime = _filebase.Times.FirstOrDefault(t => t.Id == timeToUpdate.Id);

            if (existingTime == null)
            {
                return null;
            }

            existingTime.Date = timeToUpdate.Date;
            existingTime.Narrative = timeToUpdate.Narrative;
            existingTime.Hours = timeToUpdate.Hours;
            existingTime.ProjectId = timeToUpdate.ProjectId;
            existingTime.ProjectName = timeToUpdate.ProjectName;

            var employee = _filebase.Employees.FirstOrDefault(e => e.Id == timeToUpdate.EmployeeId);
            if (employee != null)
            {
                var employeeTimeToUpdate = employee.TimeRecords.FirstOrDefault(t => t.Id == timeToUpdate.Id);
                if (employeeTimeToUpdate != null)
                {
                    employeeTimeToUpdate.Date = timeToUpdate.Date;
                    employeeTimeToUpdate.Narrative = timeToUpdate.Narrative;
                    employeeTimeToUpdate.Hours = timeToUpdate.Hours;
                    employeeTimeToUpdate.ProjectId = timeToUpdate.ProjectId;
                    employeeTimeToUpdate.ProjectName = timeToUpdate.ProjectName;

                    employee.TimeRecords.Remove(employeeTimeToUpdate);

                    employee.TimeRecords.Add(existingTime);

                    _filebase.UpdateEmployee(employee);

                    Add(existingTime);
                }
                else
                {
                    throw new Exception("Time with specified ID does not exist in the employee's records");
                }
            }
            else
            {
                throw new Exception("Employee with specified ID does not exist");
            }

            return existingTime;
        }

        public bool DeleteById(string id)
        {
            var time = _filebase.Times.FirstOrDefault(t => t.Id == Convert.ToInt32(id));

            if (time != null)
            {
                var employee = _filebase.Employees.FirstOrDefault(e => e.Id == time.EmployeeId);
                if (employee != null)
                {
                    var employeeTimeToRemove = employee.TimeRecords.FirstOrDefault(t => t.Id == time.Id);
                    if (employeeTimeToRemove != null)
                    {
                        employee.TimeRecords.Remove(employeeTimeToRemove);

                        _filebase.UpdateEmployee(employee);
                    }
                    else
                    {
                        throw new Exception("Time with specified ID does not exist in the employee's records");
                    }
                }
                else
                {
                    throw new Exception("Employee with specified ID does not exist");
                }

                if (_filebase.DeleteTime(id))
                {
                    return true;
                }
                else
                {
                    throw new Exception("Time not found");
                }
            }

            return false; 
        }

        public TimeDTO GetTimeById(int id)
        {
            var time = _filebase.Times.FirstOrDefault(t => t.Id == id);

            return time;
        }

        public IEnumerable<TimeDTO> Search(string query)
        {
            return _filebase.Times.Where(t => t.Date.ToString().ToUpper().Contains(query.ToUpper())).Take(1000);
        }
    }
}
