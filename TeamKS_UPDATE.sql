--numCheckins = sum of all check-ins for that business
update business
set numCheckins = temp2.numCheckins
from (select business_id, sum(tempSum) as numCheckins
		from (select business_id, (night + evening + afternoon + morning) as tempSum
				from checkin) as temp
		group by business_id) as temp2
where business.business_id = temp2.business_id;

--review_count = number of reviews provided for the business (overwrite the values extracted from the JSON data)
update business
set review_count = temp.tempCount
from (select business_id, count(review_id) as tempCount
		from review
		group by business_id) as temp
where business.business_id = temp.business_id;

--review_rating = average of the review star ratings provided for each business
update business
set review_rating = temp.tempRating
from (select business_id, avg(stars) as tempRating
		from review
		group by business_id) as temp
where business.business_id = temp.business_id;