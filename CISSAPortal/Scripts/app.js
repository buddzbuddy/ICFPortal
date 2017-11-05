/// <reference path="typings/jquery/jquery.d.ts" />
var UnitTypeList = (function () {
    function UnitTypeList() {
        this.unitTypes = new Array();
    }
    UnitTypeList.prototype.load = function () {
        var _this = this;
        $.getJSON('http://localhost:3180/UnitTypes/GetUnitTypesTS', function (data) {
            _this.unitTypes = data;
            alert('данные загружены');
        });
    };
    UnitTypeList.prototype.displayUnitTypes = function () {
        var table = '<table class="table">';
        for (var i = 0; i < this.unitTypes.length; i++) {
            var tableRow = '<tr>' +
                '<td>' + this.unitTypes[i].Id + '</td>' +
                '<td>' + this.unitTypes[i].Name + '</td>' +
                '</tr>';
            table += tableRow;
        }
        table += '</table>';
        $('#content').html(table);
    };
    return UnitTypeList;
}());
var UnitType = (function () {
    function UnitType() {
    }
    return UnitType;
}());
window.onload = function () {
    var unitTypeList = new UnitTypeList();
    $("#loadBtn").click(function () { unitTypeList.load(); });
    $("#displayBtn").click(function () { unitTypeList.displayUnitTypes(); });
};
//# sourceMappingURL=app.js.map