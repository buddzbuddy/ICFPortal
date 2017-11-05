/// <reference path="typings/jquery/jquery.d.ts" />
class UnitTypeList {

    private unitTypes: Array<UnitType> = new Array<UnitType>();

    load(): void {

        $.getJSON('http://localhost:3180/UnitTypes/GetUnitTypesTS',
            (data) => {
                this.unitTypes = data;
                alert('данные загружены');

            });
    }

    displayUnitTypes(): void {

        var table = '<table class="table">'
        for (var i = 0; i < this.unitTypes.length; i++) {

            var tableRow = '<tr>' +
                '<td>' + this.unitTypes[i].Id + '</td>' +
                '<td>' + this.unitTypes[i].Name + '</td>' +
                '</tr>';
            table += tableRow;
        }
        table += '</table>';
        $('#content').html(table);
    }
}

class UnitType {

    Id: number;
    Name: string;
}

window.onload = () => {
    var unitTypeList: UnitTypeList = new UnitTypeList();
    $("#loadBtn").click(() => { unitTypeList.load(); });
    $("#displayBtn").click(() => { unitTypeList.displayUnitTypes(); });
};