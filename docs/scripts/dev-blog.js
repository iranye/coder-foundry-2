function getFromInput() {
  return {
    "productID": getValue("productID"),
    "name": getValue("name"),
    "productNumber": getValue("productNumber"),
    "color": getValue("color"),
    "standardCost": getValue("standardCost"),
    "listPrice": getValue("listPrice"),
    "sellStartDate": new Date(getValue("sellStartDate"))
  };
}

function setInputArr(devlinks) {
  console.log(devlinks.length + " found...");
  for (var i = 0; i < devlinks.length; i++) {
    var devLink = devlinks[i];
    
    var link = devlinks[i];
    var linkElement = $('.container')[i];
    var anchorElement = $(linkElement).find('.anchor');
    anchorElement.attr("href", link.href);
    anchorElement.html(link.title);
    // link.show();
  }

}

