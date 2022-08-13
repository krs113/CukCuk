class EmployeePage{
    // Hàm khởi tạo
    constructor(gridId){
        let me = this

        // Lưu lại grid đang thao tác
        me.grid = $(`#${gridId}`); 

        // Khởi tạo sự kiện
        me.initEvents();

        // Khởi tạo form detail
        me.initFormDetail();

        // Lấy ra cấu hình các cột
        me.columnConfig = me.getColumnConfig();
        
        // Lấy dữ liệu
        me.getData();
    }

    /**
     * Lấy config các cột
     * VHLONG (13.07.2022)
     * @returns 
     */
    getColumnConfig(){
        let me = this,
            columnDefault = {
                FieldName: "",
                DataType: "String",
                EnumName: "",
                ClassName: "",
                Text: ""
            },
            columns = [];

        // Duyệt từng cột để vẽ header
        me.grid.find(".col").each(function(){
            let column = {...columnDefault},
                that = $(this);

            Object.keys(columnDefault).filter(function(proName){
                let value = that.attr(proName);

                if (value){
                    column[proName] = value;
                }

                column.Text = that.text();
            });

            columns.push(column)
        });

        return columns;
    }

    /**
    * Khởi tạo các sự kiện cho trang
    * VHLONG (13.07.2022)
    */
    initEvents(){
        let me = this;

        // Khởi tạo sự kiện cho toolbar
        me.initEventsToolbar();

        // Khởi tạo sự kiện cho table
        me.initEventsTable();

        // Khởi tạo sự kiện select row
        me.initEventSelectRow();
    }

    /**
     * Khởi tạo sự kiện cho toolbar
     * VHLONG (13.07.2022)
     */
     initEventsToolbar(){
        let me = this,
            toolbarId = me.grid.attr("Toolbar");

        // Khởi tạo sự kiện cho các button trên toolbar
        $(`#${toolbarId} [CommandType]`).off("click");
        $(`#${toolbarId} [CommandType]`).on("click", function(){
            let commandType = $(this).attr("CommandType");

            // gọi đến hàm động
            if (me[commandType] && typeof me[commandType] == "function"){
                me[commandType]();
            }
        });
     }

     /**
     * Khởi tạo sự kiện cho table
     * VHLONG (13.07.2022)
     */
    initEventsTable(){
        let me = this;

        me.grid.off("click", "tr");
        me.grid.on("click", "tr", function(){
            me.grid.find(".active-tr").removeClass("active-tr");

            $(this).addClass("active-tr");
        });
    }

    /**
     * Khởi tạo sự kiện select dòng
     * VHLONG (13.07.2022)
     */
    initEventSelectRow(){
        let me = this;

        me.grid.on("click","tbody tr", function(){
            $(".selectedRow").removeClass("selectedRow")

            $(this).addClass("selectedRow");
        });
    }

    /**
     *  Khởi tạo trang detail
     * VHLONG (13.07.2022)
     */
     initFormDetail(){
        let me = this;

        // Khởi tạo đối tượng form detail
        me.formDetail = new EmployeeDetail("EmployeeDetail")
    }

    /**
     * Hàm dùng để lấy dữ liệu cho trang
     */
    getData(){
        let me =this,
            url = me.grid.attr("Url")

        CommonFn.Ajax(url, Resource.Method.Get, {}, function(response){
            if(response){
                me.loadData(response.data);
            }else{
                console.log("Có lỗi khi lấy dữ liệu từ server");
            }
        });
    }

    /**
     * Load dữ liệu
     */
    loadData(data){
        let me = this;

        if(data){
            // Render dữ liệu cho grid
            me.renderGrid(data);
        }
    }

    /**
     * Render dữ liệu cho grid
     */
    renderGrid(data){
        let me = this,
            table = $("<table></table>"),
            thead = me.renderThead(),
            tbody = me.renderTbody(data);

        table.append(thead);
        table.append(tbody);

        me.grid.find("table").remove();
        me.grid.append(table);

        me.afterBinding();
    }

    /**
     * Render header
     */
    renderThead(){
        let me = this,
            thead = $("<thead></thead>"),
            tr = $("<tr></tr>");

        me.columnConfig.filter(function(column){
            let text = column.Text,
                dataType = column.DataType,
                className = me.getClassFormat(dataType),
                th = $("<th></th>");

            th.text(text);
            th.addClass(className);

            tr.append(th);
        });

        thead.append(tr);
        return thead;
    }

    /**
     * Render body
     */
    renderTbody(data){
        let me = this,
            tbody = $("<tbody></tbody>");

        if(data){
            data.filter(function(item){
                let tr = $("<tr></tr>");

                // Duyệt từng cột để để lấy thông tin các cột
                me.grid.find(".col").each(function(){
                    let column = $(this),
                        fieldName = column.attr("FieldName"),
                        dataType = column.attr("DataType"),
                        td = $("<td></td>"),
                        valueCell = item[fieldName],
                        className = me.getClassFormat(dataType),
                        value = me.getValue(valueCell, dataType, column);

                        td.text(value);
                        td.addClass(className);

                    tr.append(td);
                });

                // Lưu data lại để dùng sau
                tr.data("data", item);

                tbody.append(tr);
            });
        }

        return tbody;
    }

    /**
     * Lấy giá trị ô
     * @param {*} item 
     * @param {*} fieldName 
     * @param {*} dataType 
     */
    getValue(data, dataType, column){
        let me =this;

        switch(dataType){
            case Resource.DataTypeColumn.Number:
                data = CommonFn.formatMoney(data); 
                break;
            case Resource.DataTypeColumn.Date:
                data = CommonFn.formatDate(data);
                break;
            case Resource.DataTypeColumn.Enum:
                let enumName = column.attr("EnumName");
                data = CommonFn.getValueEnum(data, enumName);
                break;
        }

        return data;
    }

    /**
     * Hàm dùng để lấy class format cho từng kiểu dữ liệu
     */
     getClassFormat(dataType){
        let className = "";
    
        switch(dataType){
            case Resource.DataTypeColumn.Number:
                className = "align-right";
                break;
            case Resource.DataTypeColumn.Date:
                className = "align-center";
                break;
        }
    
        return className;
    }

    /**
     * Xử lý một số thứ sau khi binding xong
     */
     afterBinding(){
        let me = this;

        // Lấy Id để phân biệt các bản ghi
        me.ItemId = me.grid.attr("ItemId");

        // Mặc định chọn dòng đầu tiên
        me.grid.find("tbody tr").eq(0).addClass("selectedRow");
    }


    /**
     * Lấy ra bản ghi được select
     */
    getSelectedRecord(){
        let me = this,
            data = me.grid.find(".selectedRow").eq(0).data("data");
        return data;
    }

     /**
      * Hàm thêm mới
      * VHLONG (13.07.2022)
      */
    add(){
        let me = this,
            param = {
                parent: this,
                formMode: Enumeration.FormMode.Add,
                Record: {}
            };

        // Kiểm tra có forrm detail chưa
        if (me.formDetail){
            me.formDetail.open(param);
        }
    }

    /**
     * Hàm sửa
     * VHLONG (13.07.2022)
     */
    edit(){
        let me = this,
            param = {
                Parent: this,
                FormMode: Enumeration.FormMode.Edit,
                ItemId: me.ItemId,
                Record: {...me.getSelectedRecord()}
            };

        // Nếu có form detail thì show form
        if(me.formDetail){
            me.formDetail.open(param);
        }
    }

    /**
     * Hàm xóa
     * VHLONG (13.07.2022)
     */
    delete(){
        debugger
    }

    /**
     * Hàm nạp mới dữ liệu
     */
    refresh(){
        let me = this;

        me.getData();
    }
    
}


//Khởi tạo một biến cho trang nhân viên
var employeePage = new EmployeePage("gridEmployee ");