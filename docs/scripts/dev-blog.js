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
  console.log("setInputArr");
  for (var i = 0; i < devlinks.length; i++) {
    // console.log("i: " + i + " - " + products[i].name + " - " + products[i].productNumber);
    console.log(devlinks[i].name);
  }
}

function clearInput() {
  setValue("productID", "0");
  setValue("name", "");
  setValue("productNumber", "");
  setValue("color", "");
  setValue("standardCost", "0");
  setValue("listPrice", "0");
  setValue("sellStartDate", new Date().toLocaleDateString());
}
