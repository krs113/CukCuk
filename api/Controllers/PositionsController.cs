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
    public class PositionsController : ControllerBase
    {
        /// <summary>
        /// API Lấy danh sách toàn bộ vị trí
        /// </summary>
        /// <returns>Danh sách vị trí</returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(List<Position>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllPosition()
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=3.0.89.182;Port=3306;Database=WDT.2022.VHLONG;Uid=dev;Pwd=12345678;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh truy vấn
                string getAllPositionCommand = "SELECT * FROM Positions;";

                // Thực hiện gọi vào DB để chạy câu lệnh truy vấn ở trên
                var position = mySqlConnection.Query<Position>(getAllPositionCommand);
                
                if (position != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, position);
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
