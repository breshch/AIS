ALTER TABLE DirectoryPosts DROP constraint "FK_dbo.DirectoryPosts_dbo.DirectoryTypeOfPosts_DirectoryTypeOfPostId"--
DROP TABLE DirectoryTypeOfPosts;
EXEC sp_RENAME 'DirectoryPosts.DirectoryTypeOfPostId', 'TypeOfPost', 'COLUMN'