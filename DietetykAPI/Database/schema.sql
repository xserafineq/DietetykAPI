create table employees
(
    employee_id serial
        primary key,
    firstname   varchar(80)           not null,
    lastname    varchar(100)          not null,
    email       varchar(200)          not null
        unique,
    password    varchar(255)
        constraint employees_password_check
            check (length((password)::text) >= 5),
    isadmin     boolean default false not null
);

create unique index employees_email_uindex
    on employees (email);

create table customers
(
    pesel               varchar(11)    not null
        primary key,
    firstname           varchar(80)    not null,
    lastname            varchar(100)   not null,
    age                 integer        not null
        constraint customers_age_check
            check (age >= 1),
    email               varchar(200)   not null,
    residential_address text           not null,
    weight_destination  numeric(10, 2) not null,
    phone               varchar(12)    not null
);

create table visits
(
    visit_code     serial
        primary key,
    employee_id    integer                                                      not null
        references employees,
    customer_pesel varchar(11)                                                  not null
        references customers,
    status         varchar(25)              default 'active'::character varying not null,
    date           timestamp with time zone default now()                       not null
);

create index ix_visits_employeeid_date
    on visits (employee_id, date);

create table notifications
(
    visit_code integer     not null
        primary key
        references visits,
    status     varchar(50) not null
);

create table medical_results
(
    visit_code  integer                             not null
        primary key
        references visits,
    weight      numeric(10, 2)                      not null
        constraint medical_results_weight_check
            check (weight > (0)::numeric),
    height      numeric(10, 2)                      not null
        constraint medical_results_height_check
            check (height > (0)::numeric),
    waistline   numeric(10, 2)                      not null
        constraint medical_results_waistline_check
            check (waistline > (0)::numeric),
    body_fat    numeric(10, 2)                      not null
        constraint medical_results_body_fat_check
            check (body_fat > (0)::numeric),
    sugar_level numeric(10, 2)                      not null
        constraint medical_results_sugar_level_check
            check (sugar_level > (0)::numeric),
    bmi         numeric(10, 2)                      not null
        constraint medical_results_bmi_check
            check (bmi > (0)::numeric),
    date        timestamp default CURRENT_TIMESTAMP not null
);

create table diets
(
    diet_id      serial
        primary key,
    pdf          bytea   not null,
    type         varchar(100),
    kcal_deficit integer not null
        constraint diets_kcal_deficit_check
            check (kcal_deficit > 0)
);

create table medical_recommendations
(
    visit_code integer                                            not null
        primary key
        references visits,
    diet_id    integer                                            not null
        references diets,
    note       text,
    date       timestamp with time zone default CURRENT_TIMESTAMP not null
);

create table "__EFMigrationsHistory"
(
    "MigrationId"    varchar(150) not null
        constraint "PK___EFMigrationsHistory"
            primary key,
    "ProductVersion" varchar(32)  not null
);

create table config
(
    visit_duration integer default 45,
    id             integer
);


