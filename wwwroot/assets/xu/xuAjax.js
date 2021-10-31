document.getElementById('morebntInfo').addEventListener('click', loadmorebntInfo);
document.getElementById('moreazInfo').addEventListener('click', loadmoreazInfo);
document.getElementById('moremodernaInfo').addEventListener('click', loadmoremodernaInfo);
document.getElementById('moremvcInfo').addEventListener('click', loadmoremoremvcInfo);

function loadmorebntInfo() {

    var xhr = new XMLHttpRequest();
    xhr.open('GET', 'https://raw.githubusercontent.com/MsitTeam1/VaccineInfoAPI/main/vaccine.json', true);

    xhr.onload = function () {
        if (this.status == 200) {
            var vaccineInfo = JSON.parse(xhr.responseText);

            var bnt_output = '<ul>' +
                '<li>製造廠： '   + vaccineInfo[0].Manufacturing + '</li>' +
                '<li>疫苗成分： ' + vaccineInfo[0].MainIngredient + '</li>' +
                '<li>疫苗種類： ' + vaccineInfo[0].TypesOfVaccines + '</li>' +
                '<li>適用年齡： ' + vaccineInfo[0].SuitableAge + '</li>' +
                '<li>接種劑次： ' + vaccineInfo[0].TimesNeed + '</li>' +
                '<li>接種間隔： ' + vaccineInfo[0].TimeInterval + '</li>' +
                '<li>冷儲條件： ' + vaccineInfo[0].ColdStored1 + vaccineInfo[0].ColdStored2 + '</li>' +
                '</ul>';
            document.getElementById('bntInfo').innerHTML = bnt_output;
        }
    }
    xhr.send();
}
function loadmoreazInfo() {

    var xhr = new XMLHttpRequest();
    xhr.open('GET', 'https://raw.githubusercontent.com/MsitTeam1/VaccineInfoAPI/main/vaccine.json', true);

    xhr.onload = function () {
        if (this.status == 200) {
            var vaccineInfo = JSON.parse(xhr.responseText);

            var az_output = '<ul>' +
                '<li>製造廠： ' + vaccineInfo[1].Manufacturing + '</li>' +
                '<li>疫苗成分： ' + vaccineInfo[1].MainIngredient + '</li>' +
                '<li>疫苗種類： ' + vaccineInfo[1].TypesOfVaccines + '</li>' +
                '<li>適用年齡： ' + vaccineInfo[1].SuitableAge + '</li>' +
                '<li>接種劑次： ' + vaccineInfo[1].TimesNeed + '</li>' +
                '<li>接種間隔： ' + vaccineInfo[1].TimeInterval + '</li>' +
                '<li>冷儲條件： ' + vaccineInfo[1].ColdStored1 + vaccineInfo[1].ColdStored2 + '</li>' +
                '</ul>';
            document.getElementById('azInfo').innerHTML = az_output;
        }
    }
    xhr.send();
}
function loadmoremodernaInfo() {

    var xhr = new XMLHttpRequest();
    xhr.open('GET', 'https://raw.githubusercontent.com/MsitTeam1/VaccineInfoAPI/main/vaccine.json', true);

    xhr.onload = function () {
        if (this.status == 200) {
            var vaccineInfo = JSON.parse(xhr.responseText);

            var moderna_output = '<ul>' +
                '<li>製造廠： ' + vaccineInfo[2].Manufacturing + '</li>' +
                '<li>疫苗成分： ' + vaccineInfo[2].MainIngredient + '</li>' +
                '<li>疫苗種類： ' + vaccineInfo[2].TypesOfVaccines + '</li>' +
                '<li>適用年齡： ' + vaccineInfo[2].SuitableAge + '</li>' +
                '<li>接種劑次： ' + vaccineInfo[2].TimesNeed + '</li>' +
                '<li>接種間隔： ' + vaccineInfo[2].TimeInterval + '</li>' +
                '<li>冷儲條件： ' + vaccineInfo[2].ColdStored1 + vaccineInfo[2].ColdStored2 + '</li>' +
                '</ul>';
            document.getElementById('modernaInfo').innerHTML = moderna_output;
        }
    }
    xhr.send();
}
function loadmoremoremvcInfo() {

    var xhr = new XMLHttpRequest();
    xhr.open('GET', 'https://raw.githubusercontent.com/MsitTeam1/VaccineInfoAPI/main/vaccine.json', true);

    xhr.onload = function () {
        if (this.status == 200) {
            var vaccineInfo = JSON.parse(xhr.responseText);

            var mvc_output = '<ul>' +
                '<li>製造廠： ' + vaccineInfo[3].Manufacturing + '</li>' +
                '<li>疫苗成分： ' + vaccineInfo[3].MainIngredient + '</li>' +
                '<li>疫苗種類： ' + vaccineInfo[3].TypesOfVaccines + '</li>' +
                '<li>適用年齡： ' + vaccineInfo[3].SuitableAge + '</li>' +
                '<li>接種劑次： ' + vaccineInfo[3].TimesNeed + '</li>' +
                '<li>接種間隔： ' + vaccineInfo[3].TimeInterval + '</li>' +
                '<li>冷儲條件： ' + vaccineInfo[3].ColdStored1 + vaccineInfo[3].ColdStored2 + '</li>' +
                '</ul>';
            document.getElementById('mvcInfo').innerHTML = mvc_output;
        }
    }
    xhr.send();
}