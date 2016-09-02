var app;
(function () {
    app = angular.module('FSBrowser', []);
})();


app.service('FSBService', function ($http) {

    this.getFSDrives = function () {
        return $http.get("/api/FSObjects");
    };

    this.getDirsForDrive = function (drvLetter) {
        return $http.get("/api/FSObjects/" + drvLetter);
    };

    this.getSubDirsFor = function (dir) {
        return $http.post("/api/FSObjects/" +
            dir.ParentPath.substring(0, 1) + "/SubDirs", dir);
    };
});


app.controller('FSBControl', function ($scope, FSBService) {

    getDrives();

    function getDrives() {
        var getDrivesList = FSBService.getFSDrives();
        getDrivesList.then(function (d) {
            $scope.fsDrives = d.data;
            $scope.fsDirs = null;
        }, function (error) {
            console.log('Something is wrong with getting data from API');
        });
    };


    $scope.driveClick = function (drive) {
        var getDriveInfo = FSBService.getDirsForDrive(drive.substring(0, 1));
        getDriveInfo.then(function (d) {
            $scope.fsDirs = d.data;
            $scope.fsDrives = null;

            $scope.parentPath = null;
            $scope.currentPath = drive;

        }, function (error) {
            console.log('Something is wrong with getting data from API');
        });
    };


    $scope.dirsClick = function (dir) {
        if (dir.IsDirectory) {
            var getDirInfo = FSBService.getSubDirsFor(dir);
            getDirInfo.then(function (d) {
                $scope.fsDirs = d.data;
                $scope.fsDrives = null;

                $scope.parentPath = $scope.currentPath;

                if ($scope.currentPath.length < 4) {
                    $scope.currentPath = $scope.currentPath + dir.Name;
                } else {
                    $scope.currentPath = $scope.currentPath + "\\" + dir.Name;
                }

            }, function (error) {
                console.log('Something is wrong with getting data from API');
            });
        }
    };


    $scope.parentClick = function (parentDirPath) {

        if (parentDirPath.length < 4) {
            $scope.driveClick(parentDirPath);
        } else {

            //var parentDir = {
            //    IsDirectory: true,
            //    ParentPath: parentDirPath,
            //    Name: ""
            //};

            //var getDirInfo = FSBService.getSubDirsFor(parentDir);

            //getDirInfo.then(function (d) {
            //    $scope.fsDirs = d.data;
            //    $scope.fsDrives = null;


            //$scope.currentPath = $scope.parentPath;
            //$scope.parentPath = null;

            //}, function (error) {
            //    console.log('Something is wrong with getting data from API');
            //});

            //Current path: C:\Portable\Documents
            //Parent path: C:\Portable

            //...

            //    Music C:\\Portable\Documents - DIR - 1 0 0 
        };




        //function countFilesBySize() {
        //    var lessThanTen = 0;
        //    var notMoreFifty = 0;
        //    var moreHandred = 0;
        //    for (var i = 0; i < $scope.fsDirs.length; i++) {
        //        var file = $scope.fsDirs[i];
        //        if (!file.IsDirectory) {
        //            if (file.FileSize < 10 * 1024 * 1024) { lessThanTen++; }
        //            else if (file.FileSize <= 50 * 1024 * 1024) { notMoreFifty++; }
        //            else if (file.FileSize > 100 * 1024 * 1024) { moreHandred++; }
        //        }
        //    }
        //    $scope.lessThanTen = lessThanTen;
        //    $scope.notMoreFifty = notMoreFifty;
        //    $scope.moreHandred = moreHandred;
        //};
    };
});