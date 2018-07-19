--find most recent review for each user 
select user_id, max(date_review) from review group by user_id

--find reviews by specified user's friends
select usertable.name, business.name, business.city, reviews.text from business, usertable,(select * from review, (select friend_id from hasfriends where user_id = 'EZbvMdvCA2D9vF6VjILYXA') as friends where review.user_id = friends.friend_id) as reviews where business.business_id = reviews.business_id and usertable.user_id = reviews.user_id;

--query for friends tips grid
select usertable.name, business.name, business.city, reviews.text from business, usertable, (select reviews1.user_id, reviews1.date_review, review.text, review.business_id from review, (select review.user_id, max(review.date_review) as date_review from review, (select friend_id from hasfriends where user_id = 'EZbvMdvCA2D9vF6VjILYXA') as friends where review.user_id = friends.friend_id group by review.user_id) as reviews1 where reviews1.date_review = review.date_review and reviews1.user_id = review.user_id) as reviews where business.business_id = reviews.business_id and usertable.user_id = reviews.user_id;