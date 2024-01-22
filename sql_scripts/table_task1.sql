CREATE TABLE "data" (
    id BIGSERIAL PRIMARY KEY,
    date DATE,
    latin_chars VARCHAR(10),
    russian_chars VARCHAR(10),
    positive_even_number INT,
    positive_double_number DOUBLE PRECISION
);