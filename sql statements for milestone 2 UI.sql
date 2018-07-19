--Retrieve all distinct states that appear in the Yelp business data and list them
select distinct state_name from business

--when a user selects a state, retrieve and display the cities that appear in the Yelp business data in the selected state
--(using NC as example)
select distinct city
from business
where state_name = 'NC'

--when a city is selected, retrieve the zipcodes that appear in the Yelp business data in the selected city
--(using Ballantyne as an example)
select distinct postal_code
from business
where city = 'Ballantyne'

--when a zipcode is selected, retrieve all the business categories for the businesses that appear in that zipcode
--(using 28277 as an example)
select distinct category
from business, category
where postal_code = '28277' and business.business_id = category.business_id

--when the user searches for businesses, all the businesses in the selected zipcode will be displayed
--(using 28277 as an example)
select name --not sure which fields we need to return yet
from business
where postal_code = '28277'

--if the user selects any categories, the search results will be filtered based on the selected business categories
--(using pizza and 28277 as an example)
select name, category
from business, category
where business.business_id = category.business_id and category = 'Pizza' and postal_code = '28277'

--accomodate more than one category would follow pattern like this: (not needed in milestone 2 but needed in milestone 3)
select name, category
from business, category
where business.business_id = category.business_id and (category = 'Pizza' or category = 'Restaurants') and postal_code = '28277'