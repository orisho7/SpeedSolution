-- Create Users table
create table public.users (
  id uuid not null primary key default gen_random_uuid(),
  first_name text,
  last_name text,
  phone text,
  email text,
  password text, -- Note: In production, consider using Supabase Auth (gotrue) instead of a custom table for better security
  date_of_birth date,
  gender text,
  city text,
  referral_source text,
  created_at timestamptz default now()
);

-- Create Engineers table
create table public.engineers (
  id uuid not null primary key default gen_random_uuid(),
  first_name text,
  last_name text,
  category text,
  degree text,
  specialization text,
  schedule text,
  description text,
  rating numeric,
  image_url text,
  price numeric,
  created_at timestamptz default now()
);

-- Create Bookings table
create table public.bookings (
  id uuid not null primary key default gen_random_uuid(),
  user_id uuid references public.users(id),
  engineer_id uuid references public.engineers(id),
  booking_date date,
  booking_hour text,
  engineer_name text, -- Denormalized for simpler display
  payment_method text,
  payment_status text,
  created_at timestamptz default now()
);

-- Enable Row Level Security (RLS) - Optional but recommended
alter table public.users enable row level security;
alter table public.engineers enable row level security;
alter table public.bookings enable row level security;

-- Create policies (Simple open policies for development - TIGHTEN THESE FOR PRODUCTION)
create policy "Enable all access for all users" on public.users for all using (true) with check (true);
create policy "Enable all access for all users" on public.engineers for all using (true) with check (true);
create policy "Enable all access for all users" on public.bookings for all using (true) with check (true);

-- Seed a test user for login testing
insert into public.users (first_name, last_name, phone, email, password, date_of_birth, gender, city, referral_source)
values ('Test', 'User', '0501234567', 'test@example.com', 'password123', '1990-01-01', 'Male', 'Riyadh', 'Website');

-- Seed initial Engineers data (30 engineers for 6 per page x 5 pages)
insert into public.engineers (first_name, last_name, category, degree, specialization, schedule, description, rating, image_url, price)
values 
-- Plumbing (5 engineers)
('Ahmed', 'Ali', 'Plumbing', 'Plumbing Certification', 'Residential Plumbing', '9am - 5pm', 'Expert in residential plumbing systems and repairs.', 4.8, '/images/Eng.jpg', 80),
('Sara', 'Noor', 'Plumbing', 'Master Plumber', 'Commercial Plumbing', '10am - 6pm', 'Specialized in commercial plumbing installations.', 4.9, '/images/Eng.jpg', 100),
('John', 'Smith', 'Plumbing', 'Plumbing Tech', 'Emergency Repairs', '24/7', 'Available for emergency plumbing services.', 4.5, '/images/Eng.jpg', 90),
('Maria', 'Garcia', 'Plumbing', 'Licensed Plumber', 'Water Heaters', '8am - 4pm', 'Specialist in water heater installation and repair.', 4.7, '/images/Eng.jpg', 85),
('David', 'Lee', 'Plumbing', 'Plumbing Engineer', 'Pipe Systems', '9am - 5pm', 'Expert in complex pipe system design.', 4.6, '/images/Eng.jpg', 95),

-- Electrical (5 engineers)
('Michael', 'Brown', 'Electrical', 'B.Sc Electrical', 'Residential Wiring', '8am - 4pm', 'Certified electrician for home wiring projects.', 4.8, '/images/Eng.jpg', 120),
('Emily', 'Wilson', 'Electrical', 'Master Electrician', 'Industrial Systems', '9am - 5pm', 'Industrial electrical systems specialist.', 4.9, '/images/Eng.jpg', 150),
('James', 'Taylor', 'Electrical', 'Electrical Tech', 'Solar Installation', '10am - 6pm', 'Expert in solar panel installation and maintenance.', 4.7, '/images/Eng.jpg', 130),
('Linda', 'Martinez', 'Electrical', 'Licensed Electrician', 'Smart Home', '9am - 5pm', 'Specialist in smart home electrical systems.', 4.6, '/images/Eng.jpg', 110),
('Robert', 'Anderson', 'Electrical', 'Electrical Engineer', 'Power Systems', '8am - 3pm', 'Power distribution and backup systems expert.', 4.5, '/images/Eng.jpg', 140),

-- Construction (5 engineers)
('William', 'Thomas', 'Construction', 'Civil Engineering', 'Structural Analysis', '9am - 5pm', 'Expert in structural integrity and building foundations.', 4.8, '/images/Eng.jpg', 150),
('Jennifer', 'Jackson', 'Construction', 'Construction Management', 'Project Management', '8am - 6pm', 'Experienced construction project manager.', 4.9, '/images/Eng.jpg', 180),
('Christopher', 'White', 'Construction', 'Civil Engineer', 'Concrete Work', '7am - 3pm', 'Specialist in concrete structures and foundations.', 4.7, '/images/Eng.jpg', 160),
('Patricia', 'Harris', 'Construction', 'Structural Engineer', 'Renovation', '9am - 5pm', 'Expert in building renovation and restoration.', 4.6, '/images/Eng.jpg', 170),
('Daniel', 'Martin', 'Construction', 'Construction Tech', 'Framing', '8am - 4pm', 'Professional framing and structural work.', 4.5, '/images/Eng.jpg', 140),

-- Architecture (5 engineers)
('Matthew', 'Thompson', 'Architecture', 'M.Arch', 'Sustainable Design', '10am - 4pm', 'Focus on eco-friendly and sustainable designs.', 4.9, '/images/Eng.jpg', 200),
('Jessica', 'Garcia', 'Architecture', 'B.Arch', 'Residential Design', '9am - 5pm', 'Specialized in modern residential architecture.', 4.8, '/images/Eng.jpg', 190),
('Anthony', 'Martinez', 'Architecture', 'M.Arch', 'Commercial Design', '10am - 6pm', 'Expert in commercial building design.', 4.7, '/images/Eng.jpg', 210),
('Sarah', 'Robinson', 'Architecture', 'B.Arch', 'Interior Architecture', '9am - 5pm', 'Specialist in interior space design.', 4.6, '/images/Eng.jpg', 180),
('Joshua', 'Clark', 'Architecture', 'M.Arch', 'Urban Planning', '8am - 4pm', 'Urban planning and landscape architecture.', 4.5, '/images/Eng.jpg', 195),

-- HVAC (5 engineers)
('Andrew', 'Rodriguez', 'HVAC', 'HVAC Certification', 'Air Conditioning', '9am - 5pm', 'Expert in AC installation and maintenance.', 4.8, '/images/Eng.jpg', 100),
('Elizabeth', 'Lewis', 'HVAC', 'HVAC Tech', 'Heating Systems', '8am - 4pm', 'Specialist in heating system repair and installation.', 4.7, '/images/Eng.jpg', 95),
('Ryan', 'Walker', 'HVAC', 'Master HVAC', 'Commercial HVAC', '10am - 6pm', 'Commercial HVAC systems expert.', 4.9, '/images/Eng.jpg', 120),
('Michelle', 'Hall', 'HVAC', 'HVAC Engineer', 'Ventilation', '9am - 5pm', 'Ventilation and air quality specialist.', 4.6, '/images/Eng.jpg', 105),
('Kevin', 'Allen', 'HVAC', 'HVAC Tech', 'Energy Efficiency', '8am - 3pm', 'Focus on energy-efficient HVAC solutions.', 4.5, '/images/Eng.jpg', 110),

-- Carpentry (5 engineers)
('Brian', 'Young', 'Carpentry', 'Master Carpenter', 'Custom Furniture', '9am - 5pm', 'Expert in custom furniture and cabinetry.', 4.8, '/images/Eng.jpg', 90),
('Nicole', 'Hernandez', 'Carpentry', 'Carpentry Tech', 'Finish Carpentry', '10am - 6pm', 'Specialist in finish carpentry and trim work.', 4.7, '/images/Eng.jpg', 85),
('Jason', 'King', 'Carpentry', 'Licensed Carpenter', 'Framing', '7am - 3pm', 'Professional framing and rough carpentry.', 4.6, '/images/Eng.jpg', 80),
('Amanda', 'Wright', 'Carpentry', 'Master Carpenter', 'Restoration', '9am - 5pm', 'Antique furniture restoration specialist.', 4.9, '/images/Eng.jpg', 95),
('Brandon', 'Lopez', 'Carpentry', 'Carpentry Engineer', 'Deck Building', '8am - 4pm', 'Expert in outdoor deck and patio construction.', 4.5, '/images/Eng.jpg', 88);

