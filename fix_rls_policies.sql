-- Fix RLS Policies for SpeedSolution
-- Run this in your Supabase SQL Editor

-- First, drop any existing policies that might be conflicting
DROP POLICY IF EXISTS "Enable all access for all users" ON public.users;
DROP POLICY IF EXISTS "Enable all access for all users" ON public.engineers;
DROP POLICY IF EXISTS "Enable all access for all users" ON public.bookings;

-- Option 1: DISABLE RLS (Recommended for Development)
-- Uncomment these lines to completely disable RLS
ALTER TABLE public.users DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.engineers DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.bookings DISABLE ROW LEVEL SECURITY;

-- Option 2: CREATE PROPER RLS POLICIES (For Production)
-- Comment out the DISABLE lines above and uncomment these if you want RLS enabled
-- Make sure RLS is enabled
-- ALTER TABLE public.users ENABLE ROW LEVEL SECURITY;
-- ALTER TABLE public.engineers ENABLE ROW LEVEL SECURITY;
-- ALTER TABLE public.bookings ENABLE ROW LEVEL SECURITY;

-- Create permissive policies for anon and authenticated users
-- CREATE POLICY "Allow all operations for anon users" ON public.users
--   FOR ALL
--   TO anon
--   USING (true)
--   WITH CHECK (true);

-- CREATE POLICY "Allow all operations for authenticated users" ON public.users
--   FOR ALL
--   TO authenticated
--   USING (true)
--   WITH CHECK (true);

-- CREATE POLICY "Allow all operations for anon users" ON public.engineers
--   FOR ALL
--   TO anon
--   USING (true)
--   WITH CHECK (true);

-- CREATE POLICY "Allow all operations for authenticated users" ON public.engineers
--   FOR ALL
--   TO authenticated
--   USING (true)
--   WITH CHECK (true);

-- CREATE POLICY "Allow all operations for anon users" ON public.bookings
--   FOR ALL
--   TO anon
--   USING (true)
--   WITH CHECK (true);

-- CREATE POLICY "Allow all operations for authenticated users" ON public.bookings
--   FOR ALL
--   TO authenticated
--   USING (true)
--   WITH CHECK (true);

-- Verify the changes
SELECT schemaname, tablename, rowsecurity 
FROM pg_tables 
WHERE schemaname = 'public' 
AND tablename IN ('users', 'engineers', 'bookings');

-- Check existing policies
SELECT schemaname, tablename, policyname, permissive, roles, cmd, qual, with_check
FROM pg_policies
WHERE schemaname = 'public'
AND tablename IN ('users', 'engineers', 'bookings');
