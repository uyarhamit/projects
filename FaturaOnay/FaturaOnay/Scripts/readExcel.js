
$(document).on("click", "#upload", function () {
    //Reference the FileUpload element.
    var fileUpload = $("#fileUpload")[0];

    //Validate whether File is valid Excel file.
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx)$/;
    if (regex.test(fileUpload.value.toLowerCase())) {
        if (typeof (FileReader) != "undefined") {
            var reader = new FileReader();

            //For Browsers other than IE.
            if (reader.readAsBinaryString) {
                reader.onload = function (e) {
                    ProcessExcel(e.target.result);
                };
                reader.readAsBinaryString(fileUpload.files[0]);
            } else {
                //For IE Browser.
                reader.onload = function (e) {
                    var data = "";
                    var bytes = new Uint8Array(e.target.result);
                    for (var i = 0; i < bytes.byteLength; i++) {
                        data += String.fromCharCode(bytes[i]);
                    }
                    ProcessExcel(data);
                };
                reader.readAsArrayBuffer(fileUpload.files[0]);
            }
        } else {
            alert("This browser does not support HTML5.");
        }
    } else {
        alert("Lütfen geçerli excel dosyası yükleyiniz.");
    }
});
function ProcessExcel(data) {
    //Read the Excel File data.
    var workbook = XLSX.read(data, {
        type: 'binary'
    });

    //Fetch the name of First Sheet.
    var firstSheet = workbook.SheetNames[0];

    //Read all rows from First Sheet into an JSON array.
    var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[firstSheet]);

    //Create a HTML Table element.
    var table = $("<table />");
    table[0].border = "1";

    //Add the header row.
    var row = $(table[0].insertRow(-1));

    //Add the header cells.
    var headerCell = $("<th />");
    headerCell.html("Fatura No");
    row.append(headerCell);

    var headerCell = $("<th />");
    headerCell.html("Unvan");
    row.append(headerCell);

    var headerCell = $("<th />");
    headerCell.html("Tutar");
    row.append(headerCell);

    //Add the data rows from Excel file.
    for (var i = 0; i < excelRows.length; i++) {
        //Add the data row.
        var row = $(table[0].insertRow(-1));

        //Add the data cells.
        var cell = $("<td />");
        cell.html(excelRows[i].Fatura_No);
        row.append(cell);

        cell = $("<td />");
        cell.html(excelRows[i].Unvan);
        row.append(cell);

        cell = $("<td />");
        cell.html(excelRows[i].Tutar);
        row.append(cell);
    }

    var dvExcel = $("#dvExcel");
    dvExcel.html("");
    dvExcel.append(table);
};