// Resource dùng chung toàn chương trình
var Resource = Resource || {};

// Các kiểu dữ liệu của column trong grid
Resource.DataTypeColumn = {
    Number: "Number",
    Date: "Date",
    Enum: "Enum",
    String: "String"
};

// Các method khi gọi ajax
Resource.Method = {
    Get: "Get",
    Post: "Post",
    Put: "Put",
    Delete: "Delete"
}


// Giới tính
Resource.Gender = {
    Female: "Nữ",
    Male: "Nam",
    Other: "Khác"
}

// Tình trạng công việc
Resource.WorkStatus = {
    NotWork: "Chưa làm việc",
    MaCurrentlyWorkingle: "Đang làm việc",
    StopWork: "Ngừng làm việc",
    Retired: "Đã nghỉ việc"
}

// Các commandType của toolbar
Resource.CommandType = {
    Add: "Add",
    Edit: "Edit",
    Delete: "Delete",
    Refresh: "Refresh",
    Print: "Print",
    Duplicate: "Duplicate",
    Import: "Import",
    Export: "Export"
}

// Các action trên form detail
Resource.CommandForm = {
    Save: "Save",
    Cancel: "Cancel"
}