CREATE SCHEMA tb;

CREATE TABLE tb.file_names (
	id BIGSERIAL PRIMARY KEY,
	name VARCHAR(255)
);

CREATE TABLE tb.banks (
	id BIGSERIAL PRIMARY KEY,
	name VARCHAR(100),
	file_name_id BIGINT REFERENCES tb.file_names (id)
);

CREATE TABLE tb.periods (
	id BIGSERIAL PRIMARY KEY,
	start_date DATE,
	end_date DATE,
	file_name_id BIGINT REFERENCES tb.file_names (id)
);

CREATE TABLE tb.account_classes (
	id BIGSERIAL PRIMARY KEY,
	bank_id BIGINT REFERENCES tb.banks (id),
	period_id BIGINT REFERENCES tb.periods (id),
	number SMALLINT,
	description VARCHAR(255)
);

CREATE TABLE tb.accounts (
	id BIGSERIAL PRIMARY KEY,
	account_class_id BIGINT REFERENCES tb.account_classes (id),
	number SMALLINT
);

CREATE TABLE tb.opening_balances (
	id BIGSERIAL PRIMARY KEY,
	account_id BIGINT REFERENCES tb.accounts (id),
	assets DECIMAL(20, 2),
	liabilities DECIMAL(20, 2)
);

CREATE TABLE tb.transactions (
	id BIGSERIAL PRIMARY KEY,
	account_id BIGINT REFERENCES tb.accounts (id),
	credit DECIMAL(20, 2),
	debit DECIMAL(20, 2)
);

CREATE TABLE tb.closing_balances (
	id BIGSERIAL PRIMARY KEY,
	account_id BIGINT REFERENCES tb.accounts (id),
	assets DECIMAL(20, 2),
	liabilities DECIMAL(20, 2)
);