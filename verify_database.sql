-- Verify Database Status
-- Copy and paste this into Supabase SQL Editor and run it

-- 1. Check if RLS is actually disabled
SELECT 
    tablename, 
    rowsecurity as "RLS Enabled"
FROM pg_tables 
WHERE schemaname = 'public' 
AND tablename IN ('users', 'engineers', 'bookings');

-- 2. Count users in the table (this bypasses RLS)
SELECT COUNT(*) as "Total Users" FROM public.users;

-- 3. Show all users (limit 5)
SELECT 
    id,
    first_name,
    last_name,
    phone,
    email,
    created_at
FROM public.users
ORDER BY created_at DESC
LIMIT 5;

-- 4. Check existing RLS policies
SELECT 
    schemaname,
    tablename,
    policyname,
    permissive,
    roles,
    cmd
FROM pg_policies
WHERE schemaname = 'public'
AND tablename = 'users';

-- 5. Check table permissions
SELECT 
    grantee, 
    privilege_type 
FROM information_schema.role_table_grants 
WHERE table_schema = 'public' 
AND table_name = 'users';
