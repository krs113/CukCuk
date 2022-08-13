using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Entities;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// API Lấy toàn bộ danh sách phòng ban
        /// </summary>
        /// <returns>Danh sách phòng ban</returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(List<Department>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllDepartment()
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.VHLONG;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh truy vấn
                string getAllDepartmentCommand = "SELECT* FROM Departments;";

                // Thực hiện gọi vào DB để chạy câu lệnh truy vấn ở trên
                var department = mySqlConnection.Query<Department>(getAllDepartmentCommand);

                if (department != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, department);
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
    }
}
