CREATE FUNCTION [dbo].[InitCap] (@inStr VARCHAR(8000))
  RETURNS VARCHAR(8000)
  AS
  BEGIN
    DECLARE @outStr VARCHAR(8000) = LOWER(@inStr),
			@char CHAR(1),	
			@charEspaco CHAR(1),
			@alphanum BIT = 0,
			@len INT = LEN(@inStr),
            @pos INT = 1;		  
 
    -- Iterate through all characters in the input string
    WHILE @pos <= @len BEGIN
 
      -- Get the next character
      SET @char = SUBSTRING(@inStr, @pos, 1);
	  SET @charEspaco = SUBSTRING(@inStr, @pos+2, 1);
	
	  IF @pos = 1
	  BEGIN 
		SET @outStr = STUFF(@outStr, @pos, 1, UPPER(@char));	
	  END
	  
	  IF @charEspaco <> ' ' 
	  BEGIN
		-- If the position is first, or the previous characater is not alphanumeric
		-- convert the current character to upper case
		IF @pos = 1 OR @alphanum = 0
			SET @outStr = STUFF(@outStr, @pos, 1, UPPER(@char));
	  END

      SET @pos = @pos + 1;
 
      -- Define if the current character is non-alphanumeric
      IF ASCII(@char) <= 47 OR (ASCII(@char) BETWEEN 58 AND 64) OR
	  (ASCII(@char) BETWEEN 91 AND 96) OR (ASCII(@char) BETWEEN 123 AND 126)
	  SET @alphanum = 0;
      ELSE
	  SET @alphanum = 1;
 
    END
 
   RETURN @outStr;		   
  END



  
ALTER FUNCTION [dbo].CalcIdade(@dtNascimento datetime)
RETURNS VARCHAR(200)
BEGIN
declare @now date,@dob date, @now_i int,@dob_i int, @days_in_birth_month int
declare @years int, @months int, @days int
declare @retorno varchar(200)
SET @retorno = N'';
set @now = GETDATE() 
set @dob = @dtNascimento -- Date of Birth

set @now_i = convert(varchar(8),@now,112) -- iso formatted: 20130228
set @dob_i = convert(varchar(8),@dob,112) -- iso formatted: 20120229
set @years = ( @now_i - @dob_i)/10000
-- (20130228 - 20120229)/10000 = 0 years

set @months =(1200 + (month(@now)- month(@dob))*100 + day(@now) - day(@dob))/100 %12
-- (1200 + 0228 - 0229)/100 % 12 = 11 months

set @days_in_birth_month = day(dateadd(d,-1,left(convert(varchar(8),dateadd(m,1,@dob),112),6)+'01'))
set @days = (sign(day(@now) - day(@dob))+1)/2 * (day(@now) - day(@dob))
          + (sign(day(@dob) - day(@now))+1)/2 * (@days_in_birth_month - day(@dob) + day(@now))
-- ( (-1+1)/2*(28 - 29) + (1+1)/2*(29 - 29 + 28))
-- Explain: if the days of now is bigger than the days of birth, then diff the two days
--          else add the days of now and the distance from the date of birth to the end of the birth month 
--RETURN CONCAT(@retorno,' ',CONVERT(VARCHAR(5),@years),' Anos')
IF(@years <= 0)
BEGIN
	IF(@months >1)
	BEGIN
		SET @retorno = CONCAT(@retorno,' ',CONVERT(VARCHAR,@months),' Meses')
	END
	ELSE
	BEGIN
		SET @retorno = CONCAT(@retorno,' ',CONVERT(VARCHAR,@months),' Mês')
	END
	IF(@days >1)
		BEGIN
			SET @retorno = CONCAT(@retorno,' ',CONVERT(VARCHAR,@days),' Dias')
		END
	ELSE
		BEGIN
			SET @retorno = CONCAT(@retorno,' ',CONVERT(VARCHAR,@days),' Dia')
		END
	RETURN (@retorno)
END

IF(@years > 18)
	BEGIN
		SET @retorno = CONCAT(@retorno,' ',CONVERT(VARCHAR,@years),' Anos')
		RETURN (@retorno)
	END
IF(@years >1)
	BEGIN
		SET @retorno = CONCAT(@retorno,' ',CONVERT(VARCHAR,@years),' Anos')
	END
	ELSE
	BEGIN
		SET @retorno = CONCAT(@retorno,' ',CONVERT(VARCHAR,@years),' Ano')
	END
	IF(@months >1)
	BEGIN
		SET @retorno = CONCAT(@retorno,' ',CONVERT(VARCHAR,@months),' Meses')
	END
	ELSE
	BEGIN
		SET @retorno = CONCAT(@retorno,' ',CONVERT(VARCHAR,@months),' Mês')
	END
RETURN (@retorno)
END

GO


