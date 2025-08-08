# Updating from Version v1.x.x to v2.x.x or higher

## Info

The database of GradeMaster version v1.x.x is incompatible with the database of GradeMaster version v2.x.x or higher.

**What does this mean?**

- You are not able to update GradeMaster the usual way.
- You must manually transfer your data

## Instructions for updating

1. Copy and backup the GradeMaster.db file, located at `C:\Users\YourUser\AppData\Local\GradeMaster\Data\GradeMaster.db` to another location.
2. Then delete the GradeMaster.db file from the location you copied it from.
3. Update the application by following the standard updating manual.
4. After installation make sure the program can execute.
5. Then execute the application wait a few seconds and then close it again.
6. Open both the first GradeMaster.db file you copied to your location and then open the newly generated GradeMaster.db file located at `C:\Users\YourUser\AppData\Local\GradeMaster\Data\GradeMaster.db` with a suitable tool like: DB Browser (SQLite).
7. Then locate the Educations table from the original db file, select all rows with all columns, then copy the SQL code and paste it in to script section of the other open DB Browser (SQLite) Window with the new database and execute the SQL code.
8. Repeat step seven for the Subjects and Grades table. The order in which you do this must be like this: Educations => Subjects => Grades.
9. After Completing the steps above close the DB Browser (SQlite) with the original database. Then write the changes to the new database and close it.
10. Now you can continue using GradeMaster as before.

## Miscellaneous

Incase something went wrong you can still get the original database back by restoring the file from the trash bin.

If for some other reason or you don't want to update you can still use an older version of GradeMaster. In this case use the newest available version that remains compatible. In this case tis would be v1.0.1. Please not, that v1.x.x will no longer be maintained or updated.
