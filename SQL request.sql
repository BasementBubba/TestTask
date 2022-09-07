select productstable.productame, categorytable.categoryname
from productstable
left join categorytable
on productstable.id = categorytable.productid 