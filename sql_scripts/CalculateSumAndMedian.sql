CREATE OR REPLACE FUNCTION CalculateSumAndMedian()
RETURNS TABLE(sum_of_integers BIGINT, median_of_doubles DOUBLE PRECISION) AS $$
BEGIN
    SELECT SUM(positive_even_number) INTO sum_of_integers FROM "data";
    
    SELECT PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY positive_double_number) INTO median_of_doubles FROM "data";
    
    RETURN QUERY SELECT sum_of_integers, median_of_doubles;
END;
$$ LANGUAGE plpgsql;