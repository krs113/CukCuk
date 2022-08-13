using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Entities;
using MISA.CukCuk.Api.Entities.DTO;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        /// <summary>
        /// API Thêm mới 1 nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên thêm mới</param>
        /// <returns>ID của nhân viên vừa thêm mới</returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult InsertEmployee([FromBody]Employee employee)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.VHLONG;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh INSERT INTO
                string insertEmployeeCommand = "INSERT INTO Employees (EmployeeID, EmployeeCode, EmployeeName, DateOfBirth, Gender, IdentityNumber, IdentityIssuedPlace, IdentityIssuedDate, Email, PhoneNumber, PositionID, DepartmentID, TaxCode, Salary, JoiningDate, WorkStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)" +
                    "VALUES(@EmployeeID, @EmployeeCode, @EmployeeName, @DateOfBirth, @Gender, @IdentityNumber, @IdentityIssuedPlace, @IdentityIssuedDate, @Email, @PhoneNumber, @PositionID, @DepartmentID, @TaxCode, @Salary, @JoiningDate, @WorkStatus, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy);";

                // Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
                var dateTimeNow = DateTime.Now;
                var employeeID = Guid.NewGuid();
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityIssuedPlace", employee.IdentityIssuedPlace);
                parameters.Add("@IdentityIssuedDate", employee.IdentityIssuedDate);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionID", employee.PositionID);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@TaxCode", employee.TaxCode);
                parameters.Add("@Salary", employee.Salary);
                parameters.Add("@JoiningDate", employee.JoiningDate);
                parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@CreatedDate", dateTimeNow);
                parameters.Add("@CreatedBy", employee.CreatedBy);
                parameters.Add("@ModifiedDate", dateTimeNow);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);

                // Thực hiện gọi vào DB để chạy câu lệnh INSERT INTO với tham số đầu vào ở trên
                var numberOfAffectedRows = mySqlConnection.Execute(insertEmployeeCommand, parameters);

                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status201Created, new GuidString()
                    {
                        StringResult = employeeID
                    }) ;
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKey)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API Sửa thông tin nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID của nhân viên vừa sửa</returns>
        [HttpPut("{employeeID}")]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateEmployee([FromRoute]Guid employeeID, [FromBody]Employee employee)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.VHLONG;Uid=dev;Pwd=12345678;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh UPDATE
                string updateEmployeeCommand = "UPDATE Employees " +
                    "SET EmployeeCode = @EmployeeCode," +
                    "EmployeeName = @EmployeeName," +
                    "DateOfBirth = @DateOfBirth," +
                    "Gender = @Gender," +
                    "IdentityNumber = @IdentityNumber," +
                    "IdentityIssuedPlace = @IdentityIssuedPlace," +
                    "IdentityIssuedDate = @IdentityIssuedDate," +
                    "Email = @Email," +
                    "PhoneNumber = @PhoneNumber," +
                    "PositionID = @PositionID," +
                    "DepartmentID = @DepartmentID," +
                    "TaxCode = @TaxCode," +
                    "Salary = @Salary," +
                    "JoiningDate =@JoiningDate," +
                    "WorkStatus = @WorkStatus," +
                    "ModifiedDate = @ModifiedDate," +
                    "ModifiedBy = @ModifiedBy " +
                    "WHERE EmployeeID = @EmployeeID;";

                // Chuẩn bị tham số đầu vào cho câu lệnh UPDATE
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeID", employeeID);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@EmployeeName", employee.EmployeeName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityIssuedPlace", employee.IdentityIssuedPlace);
                parameters.Add("@IdentityIssuedDate", employee.IdentityIssuedDate);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionID", employee.PositionID);
                parameters.Add("@DepartmentID", employee.DepartmentID);
                parameters.Add("@TaxCode", employee.TaxCode);
                parameters.Add("@Salary", employee.Salary);
                parameters.Add("@JoiningDate", employee.JoiningDate);
                parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@ModifiedDate", employee.ModifiedDate);
                parameters.Add("@ModifiedBy", employee.ModifiedBy);


                // Thực hiện gọi vào DB để chạy câu lệnh UPDATE với tham số đầu vào ở trên
                int numberOfAffectedRows = mysqlConnection.Execute(updateEmployeeCommand, parameters);

                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    // Trả về dữ liệu cho client    
                    return StatusCode(StatusCodes.Status200OK, new GuidString()
                    {
                        StringResult = employeeID
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            // Bổ sung try catch để bắt exception
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry) ;
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API Xóa 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn xóa</param>
        /// <returns>ID của nhân viên vừa xóa</returns>
        [HttpDelete("{employeeID}")]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteEmployee([FromRoute]Guid employeeID)
        {
            try
            {
                //Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.VHLONG;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị câu lệnh DELETE
                string deleteEmployeeCommand = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";

                //Chuẩn bị tham số đầu vào cho câu lệnh DELETE
                var parameter = new DynamicParameters();
                parameter.Add("@EmployeeID", employeeID);

                //Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
                int numberOfAffectedRows = mySqlConnection.Execute(deleteEmployeeCommand, parameter);

                //Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    //Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, new GuidString()
                    {
                        StringResult = employeeID
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            //Bổ sung try catch để bắt exception
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API Lấy danh sách nhân viên cho phép lọc và phân trang
        /// </summary>
        /// <param name="code">Mã nhân viên</param>
        /// <param name="name">Tên nhân viên</param>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <param name="positionID">ID vị trí</param>
        /// <param name="departmentID">ID phòng ban</param>
        /// <param name="pageSize">Số trang muốn lấy</param>
        /// <param name="pageNumber">Thứ tự trang muốn lấy</param>
        /// <returns>Một đối tượng bao gồm:
        /// + Danh sách nhân viên thoải mãn điều kiện lọc và phân trang
        /// + Tổng nhân viên thoải mãn điều kiện</returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(PagingData<Employee>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterEmployee([FromQuery]string? code, [FromQuery]string? name, [FromQuery]string? phoneNumber, 
            [FromQuery]Guid? positionID, [FromQuery]Guid? departmentID, [FromQuery]int pageSize = 100, [FromQuery]int pageNumber = 1)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.VHLONG;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string storedProcedureName = "Proc_Employee_GetPaging";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameter = new DynamicParameters();
                parameter.Add("$Skip", (pageNumber - 1) * pageSize);
                parameter.Add("$Take", pageSize);
                parameter.Add("$Sort", "EmployeeCode DESC");

                var whereCondition = new List<string>();
                if (code != null)
                {
                    whereCondition.Add($"EmployeeCode LIKE '%{code}%'");
                }
                if (code != name)
                {
                    whereCondition.Add($"EmployeeNameLIKE '%{name}%'");
                }
                if (phoneNumber != null)
                {
                    whereCondition.Add($"PhoneNumber LIKE '%{phoneNumber}%'");
                }
                if (positionID != null)
                {
                    whereCondition.Add($"PositionID LIKE '%{positionID}%'");
                }
                if (departmentID != null)
                {
                    whereCondition.Add($"DepartmentID LIKE '%{departmentID}%'");
                }

                string whereClause = string.Join(" AND ", whereCondition);
                parameter.Add("$Where", whereClause);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var multipleResult = mySqlConnection.QueryMultiple(storedProcedureName, parameter, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                if (multipleResult != null)
                {
                    var employee = multipleResult.Read<Employee>();
                    var totalCount = multipleResult.Read<long>().Single();
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, new PagingData<Employee>()
                    {
                        Data = employee.ToList(),
                        TotalCount = totalCount
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            // Bổ sung try catch để bắt exception
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API Lấy thông tin chi tiết của 1 nhân viên
        /// </summary>
        /// <param name="employeeID">ID của nhân viên cần lấy thông tin chi tiết</param>
        /// <returns>Đối tượng nhân viên cần lấy thông tin chi tiết</returns>
        [HttpGet("{employeeID}")]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmployeeByID([FromRoute]Guid employeeID)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.VHLONG;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string storedProcedureName = "Proc_Employee_GetByEmployeeID";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameter = new DynamicParameters();
                parameter.Add("$EmployeeID", employeeID);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var employee = mySqlConnection.QueryFirstOrDefault(storedProcedureName, parameter, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                if (employee != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, employee);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            // Bổ sung try catch để bắt exception
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API Lấy mã nhân viên mới tự động tăng
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        [HttpGet("new-code")]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                //Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.VHLONG;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                //Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_Employee_GetMaxCode";

                //Thực hiện gọi vào DB để chạy stored procedure ở trên
                string maxEmployeeCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý sinh mã nhân viên mới tự động tăng
                //Cắt chuỗi mã nhân viên lớn nhất trong hệ thống để lấy phần số
                //Mã nhân viên mới = "NV" + Giá trị cắt chuỗi ở  trên + 1
                string newEmployeeCode = "NV" + (Int64.Parse(maxEmployeeCode.Substring(2)) + 1).ToString();

                //Trả về dữ liệu cho client
                return StatusCode(StatusCodes.Status200OK, newEmployeeCode);

            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            //Bổ sung try catch để bắt exception
        }
    }
}
