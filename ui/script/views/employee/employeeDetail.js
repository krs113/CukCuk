class EmployeeDetail{
    constructor(formId){
        let me = this;

        me.form = $(`#${formId}`);
        me.formMode = null;

        // Khởi tạo sự kiện cho form
        me.initEvents();
    }

    /**
     * Khởi tạo các sự kiện form
     * VHLONG (13.7.2022)
     */
    initEvents(){
        let me =this;

        // Khởi tạo sự kiện button trên toolbar dưới form
        me.form.find(".toolbar-form [CommandType]").off("click");
        me.form.find(".toolbar-form [CommandType]").on("click", function(){
            let commandType = $(this).attr("CommandType");

            if (me[commandType] && typeof me[commandType] == "function"){
                me[commandType]();
            }
        });
    }

    /**
     * Hàm mở form
     * VHLONG (13.7.2022)
     */
     open(param){
        let me = this;

        Object.assign(me, param);

         // Mở form và reset
         me.show();

         // Kiểm tra xem có phải mode sửa không
         if(me.FormMode == Enumeration.FormMode.Edit){
             me.bindingData(me.Record);
        }
    }

    /**
     * Binding dữ liệu form
     * VHLONG (13.07.2022)
     */
     bindingData(data){
        let me = this;

        // Duyệt từng control để binding dữ liệu
        me.form.find("[FieldName]").each(function(){
            let fieldName = $(this).attr("FieldName"),
                dataType = $(this).attr("DataType"),
                value = data[fieldName],
                control = $(this);

            me.setValueControl(control, value , dataType);
        });
    }

    /** 
     * Set giá trị cho control
     * VHLONG (13.07.2022)
     */
    setValueControl(control, value , dataType){
        let me = this;

        switch(dataType){
            case Resource.DataTypeColumn.Date:
                value = CommonFn.convertDate(value);
                break;
        }

        control.val(value);
    }

    show(){
        let me = this;

        me.form.show();

        // Reset dữ liệu
        me.resetForm();
    }

    /**
     * Reset nội dung  form
     */
     resetForm(){
        let me = this;

        me.form.find("[SetField]").each(function(){
            let dataType = $(this).attr("DataType") || "String";

            switch(dataType){
                case Resource.DataTypeColumn.Enum:
                    me.resetEnum(this);
                    break;
                case Resource.DataTypeColumn.Number:
                case Resource.DataTypeColumn.String:
                    $(this).val("");
                    break;
            }
        });
    }

    /**
     * Xử lí validate form
     * VHLONG (13.07.2022)
     */
    validateForm(){
        let me = this,
            isValid = me.validateRequire();

        if(isValid){
            isValid = me.validateFieldNumber(); // Validate các trường nhập  số
        }

        if(isValid){
            isValid = me.validateFieldDate(); // Validate các trường ngày tháng
        }

        return isValid;
    }

    /**
     * Validate các trường bắt buộc
     * VHLONG (13.07.2022)
     */
    validateRequire(){
        let me = this,
            isValid = true;

        me.form.find('[Required]').each(function(){
            let value = $(this).val();

            if(!value){
                isValid = false;

                $(this).addClass("required-control");
                $(this).attr("title", "Vui lòng không để trống!");
            }else{
                $(this).removeClass("required-control");
            }
        });

        return isValid;
    }

    /**
     * Validate các trường Number
     * VHLONG (13.07.2022)
     */
    validateFieldNumber(){
        let me = this,
            isValid = true;

        me.form.find("[DataType='Number']").each(function(){
            let value = $(this).val();

            // is Not a Number
            if(isNaN(value)){
                isValid = false;

                $(this).addClass("required-control");
                $(this).attr("title", "Vui lòng nhập đúng định dạng!");
            }else{
                $(this).removeClass("required-control");
            }
        });

        return isValid;
    }

    /**
     * Validate các trường ngày tháng
     */
    validateFieldDate(){
        let me = this,
            isValid = true;

        me.form.find("[DataType='Date']").each(function(){
            let value = $(this).val();

            if(!CommonFn.isDateFormat(value)){
                isValid = false;

                $(this).addClass("required-control");
                $(this).attr("title", "Vui lòng nhập đúng định dạng!");
            }else{
                $(this).removeClass("required-control");
            }
        });

        return isValid;
    }

    /**
     * Lấy dữ liệu form
     * VHLONG (13.07.2022)
     */
    getFormData(){
        let me = this,
            data = me.Record || {};

        me.form.find("[SetField]").each(function(){
            let dataType = $(this).attr("DataType") || "String",
                field = $(this).attr("SetField"),
                value = null;

            switch(dataType){
                case Resource.DataTypeColumn.Date:
                    value = CommonFn.convertDate(value);
                    break;
                case Resource.DataTypeColumn.String:
                    value = $(this).val();
                    break;
                case Resource.DataTypeColumn.Enum:
                case Resource.DataTypeColumn.Number:
                    if($(this).val()){
                        value = parseInt($(this).val());
                    }
                    break;
            }

            data[field] = value;
        });

        return data;
    }

    /**
     * Xử lí lưu dữ liệu
     * VHLONG (13.07.2022)
     */
    saveData(data){
        let me =this,
            method = Resource.Method.Post,
            url = me.form.attr("Url");

        // Xử lí lưu vào db
        if (me.formMode == Enumeration.FormMode.Edit){
            method = Resource.Method.Put;
        }

        CommonFn.Ajax(url, method, data, function(response){
            if(response){
                console.log("Lưu dữ liệu thành công");

                me.close();
                me.parent.getData();
            }else{
                console.log("Có lỗi");
            }
        });
    }

     /**
      * Hàm đóng forrm
      * VHLONG (13.07.2022)
      */
    close(){
        let me = this;

        me.form.hide();
    }
 
    /**
     * Hàm lưu dữ liệu
     * VHLONG (13.07.2022)
     */
     save(){
        let me = this,
            isValid = me.validateForm();

        if (isValid){
            let data = me.getFormData();

            me.saveData(data);
        }
    }

}