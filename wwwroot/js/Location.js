

$(function () {
    GetXMLProperty("/XML/taiwanCountyToTown.xml", 'county').then(function (data) {

        XMLcollectionToSection(data, '#county', 'name')

        $('#county').change(function () {
            let selectCounty = $(this).val();
            let filteredCounty = $(data).filter(function () {
                return $(this).attr('name') == selectCounty;
            })
            let townData = filteredCounty.find('area')
            XMLcollectionToSection(townData, '#town', null, 'zip')
        })
        //let road = $('#road');
        //let firstTitleOption = road.find(':first-child');
        //road.empty();
        //road.append(firstTitleOption);

        $('#town').change(GetRoadDataToRoadSection);

    });

});

/**
*
* @@param { String } url
* @@param { String } xmlPropertyName
*/
async function GetXMLProperty(url, xmlPropertyName) {
    let data = null
    await $.get(url, function (xml) {
        /*console.log($(xml).find(xmlPropertyName));*/
        data = $(xml).find(xmlPropertyName);
    });
    return data;
}

/**
 * @@param {string} collectionAttrToSectionValue
 * @@param {string} sectionSelector
 */
function XMLcollectionToSection(collection, sectionSelector, collectionAttrToText = null, collectionAttrToSectionValue = null) {
    let firstTitleOption = $(sectionSelector).find(":first-child");
    $(sectionSelector).empty();
    $(sectionSelector).append($(firstTitleOption));
    $(collection).each(function () {
        let text = collectionAttrToText ? $(this).attr(collectionAttrToText) : $(this).text();
        let value = collectionAttrToSectionValue ? $(this).attr(collectionAttrToSectionValue) : text;
        $(sectionSelector).append($('<option>', {
            text: text,
        }).val(value));
    })
}

function GetRoadDataToRoadSection() {

    //let url = 'https://www.ris.gov.tw/rs-opendata/api/v1/datastore/ODRP049/107';
    //let url = 'http://od.moi.gov.tw/od/data/api/8697AFDD-0633-49CA-88DF-4B815FCC6C88?$format=json&$top=32407'
    let url = '/XML/opendata110road.csv'

    $.get(url, function (data) {
        var jsonData = $.csv.toObjects(data);
        //let responseData = data.responseData;
        let conditionText = $('#county').val() + $('#town').find("option:selected").text();
        let road = $('#road')
        let firstTitleOption = road.find(':first-child');
        road.empty();
        road.append(firstTitleOption);

        $(jsonData).filter(function () {
            return this.site_id == conditionText;
        }).map(function () {
            return this.road;
        }).each(function () {
            text = this;           
            road.append($('<option>', {
                text: text,
            }));
        })       
    })

}

