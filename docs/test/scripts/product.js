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

function setInput(product) {
  setValue("productID", product.productID);
  setValue("name", product.name);
  setValue("productNumber", product.productNumber);
  setValue("color", product.color);
  setValue("standardCost", product.standardCost);
  setValue("listPrice", product.listPrice);
  setValue("sellStartDate", product.sellStartDate);
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
