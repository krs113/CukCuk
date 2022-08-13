using MISA.CukCuk.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace MISA.CukCuk.Api.Entities
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// ID nhân viên (dạng guid)
        /// </summary>
        public Guid EmployeeID { get; set; }

        /// <summary>
        /// Mã nhân viên 
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required(ErrorMessage = "e005")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Số CMND/CCCD
        /// </summary>
        [Required(ErrorMessage = "e006")]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Nơi cấp CMND/CCCD
        /// </summary>
        public string IdentityIssuedPlace { get; set; }

        /// <summary>
        /// Ngày cấp CMND/CCCD
        /// </summary>
        public DateTime IdentityIssuedDate { get; set; }

        /// <summary>
        /// Email nhân viên
        /// </summary>
        [Required(ErrorMessage = "e007")]
        [EmailAddress(ErrorMessage = "e009")]
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required(ErrorMessage = "e008")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Mã vị trí
        /// </summary>
        public Guid? PositionID { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public Guid? DepartmentID { get; set; }

        /// <summary>
        /// Tên Phòng ban
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Mã số thuế cá nhân
        /// </summary>
        public string TaxCode { get; set; }

        /// <summary>
        /// Lương
        /// </summary>
        public double Salary { get; set; }

        /// <summary>
        /// Ngày gia nhập
        /// </summary>
        public DateTime JoiningDate { get; set; }

        /// <summary>
        /// Tình trạng công việc
        /// </summary>
        public WorkStatus WorkStatus { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Thời gian sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
