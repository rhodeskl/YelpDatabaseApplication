-- whenever a new review is provided for a business, the review_count and review_rating values for that business
--should be automatically updated
CREATE OR REPLACE FUNCTION updateCountAndRating() 
	RETURNS trigger AS $updateCountAndRating$
BEGIN
	UPDATE Business
		SET review_count = OLD.review_count + 1
		SET review rating = (SELECT AVG(stars)
							FROM Review
							WHERE business_id = OLD.business_id
							GROUP BY business_id)
		WHERE business_id = OLD.business_id;
	RETURN NEW;
END;
$updateCountAndRating$ LANGUAGE plpgsql;

CREATE TRIGGER mustUpdateCountAndRating
AFTER INSERT OF Review
FOR EACH ROW
EXECUTE PROCEDURE updateCountAndRating();

--when a customer checks-in a business, the numCheckins value for that business should be automatically updated

--this trigger increments by any multiple number of checkins
CREATE OR REPLACE FUNCTION updateNumCheckins() 
	RETURNS trigger AS $updateNumCheckins$
BEGIN
	UPDATE Business
		SET numCheckins = (Select (sa +sn + se + sm) as total 
							from
							(select business_id, sum(afternoon) as sa , sum(night) as sn ,
									sum(morning) as sm , sum(evening) as se
							from checkin
							group by business_id) as t1
							where t1.business_id = NEW.business_id) 
        WHERE  Business.business_id = NEW.business_id;
		
	RETURN NEW;
END;
$updateNumCheckins$ LANGUAGE plpgsql;

CREATE TRIGGER mustUpdateNumCheckins
AFTER UPDATE OR INSERT ON Checkin
FOR EACH ROW
EXECUTE PROCEDURE updateNumCheckins();


/*CREATE OR REPLACE FUNCTION updateNumCheckins() 
	RETURNS trigger AS $updateNumCheckins$
BEGIN
	UPDATE Business
		SET numCheckins = numCheckins+1
		where t1.business_id = OLD.business_id);
		
	RETURN NEW;
END;
$updateNumCheckins$ LANGUAGE plpgsql;

CREATE TRIGGER mustUpdateNumCheckins
AFTER UPDATE ON Checkin
FOR EACH ROW
EXECUTE PROCEDURE updateNumCheckins();*/


--extra credit: customers can write reviews for open businesses only. Please not that a business is active if its
--is_open value is true

/*CREATE OR REPLACE FUNCTION reviewIfBusinessOpen() 
	RETURNS trigger AS $reviewIfBusinessOpen$
BEGIN
	DELETE FROM Review
		where business_id NOT IN
			(
				SELECT business_id
				from business
				where is_open = true
			)		
		
	RETURN NEW;
END;
$reviewIfBusinessOpen$ LANGUAGE plpgsql;

CREATE TRIGGER onlyReviewIfBusinessOpen
AFTER INSERT ON Review
WHEN (EXISTS
		(SELECT *
		from NEW 
		where business_id IN
			(SELECT business_id
			from business
			where(is_open = false)))
			)
FOR EACH ROW
EXECUTE PROCEDURE reviewIfBusinessOpen();*/

CREATE OR REPLACE FUNCTION reviewIfBusinessOpen() 
	RETURNS trigger AS $reviewIfBusinessOpen$
BEGIN
	DELETE FROM Review
		where business_id NOT IN
			(
				SELECT business_id
				from business
				where is_open = true
			)	;	
		
	RETURN NEW;
END;
$reviewIfBusinessOpen$ LANGUAGE plpgsql;

CREATE TRIGGER onlyReviewIfBusinessOpen
AFTER INSERT ON Review
FOR EACH STATEMENT
EXECUTE PROCEDURE reviewIfBusinessOpen();