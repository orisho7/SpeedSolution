-- COMPREHENSIVE FIX FOR USERS TABLE RLS
-- Run this ENTIRE script in Supabase SQL Editor

-- Step 1: Drop ALL existing policies on users table
DO $$ 
DECLARE
    r RECORD;
BEGIN
    FOR r IN (SELECT policyname FROM pg_policies WHERE schemaname = 'public' AND tablename = 'users') 
    LOOP
        EXECUTE 'DROP POLICY IF EXISTS ' || quote_ident(r.policyname) || ' ON public.users';
    END LOOP;
END $$;

-- Step 2: Disable RLS on users table
ALTER TABLE public.users DISABLE ROW LEVEL SECURITY;

-- Step 3: Grant full permissions to anon role (the role used by your API key)
GRANT ALL ON public.users TO anon;
GRANT ALL ON public.users TO authenticated;
GRANT ALL ON public.users TO service_role;

-- Step 4: Verify RLS is disabled
SELECT 
    tablename, 
    rowsecurity as "RLS Enabled (should be false)"
FROM pg_tables 
WHERE schemaname = 'public' 
AND tablename = 'users';

-- Step 5: Verify no policies exist
SELECT 
    COUNT(*) as "Policy Count (should be 0)"
FROM pg_policies
WHERE schemaname = 'public'
AND tablename = 'users';

-- Step 6: Test query (should return all users)
SELECT COUNT(*) as "Total Users in Database" FROM public.users;

-- Step 7: Show sample users
SELECT id, first_name, last_name, phone, email 
FROM public.users 
LIMIT 3;
