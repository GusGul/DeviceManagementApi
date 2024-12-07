-- Create a table to track applied migrations
CREATE TABLE IF NOT EXISTS public."SchemaVersions" (
    Id SERIAL PRIMARY KEY,
    ScriptName TEXT NOT NULL,
    AppliedAt TIMESTAMPTZ DEFAULT NOW() NOT NULL,
    AppliedBy TEXT NOT NULL
);

-- Create an index on the ScriptName to ensure fast lookups
CREATE INDEX IF NOT EXISTS idx_scriptname ON public."SchemaVersions" (ScriptName);
