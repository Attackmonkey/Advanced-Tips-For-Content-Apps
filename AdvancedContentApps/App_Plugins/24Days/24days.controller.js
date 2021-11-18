'use strict';
(function () {
    'use strict';
    function ArticleImportController($scope, editorState, contentResource, notificationsService, $timeout, Upload, $rootScope) {
        var vm = this;

        vm.blogKey = editorState.current.key;
        vm.fileName = '';
        vm.processing = false;
        vm.invalidFileFormat = false;
        vm.noFile = false;
        vm.file = null;

        vm.upload = function () {
            if (vm.file === null) {
                vm.noFile = true;
                $timeout(function () {
                    vm.noFile = false;
                }, 500);
                return;
            }
            vm.processing = true;

            Upload.upload({
                url: "/umbraco/backoffice/24days/import/importarticles?blogKey=" + vm.blogKey,
                file: vm.file
            }).success(function (response) {
                vm.processing = false;

                notificationsService.success(response);

                activate();
            }).error(function (data) {
                vm.processing = false;

                notificationsService.error("Unable to process CSV upload");
            });
        }

        $scope.$on("filesSelected", function (event, args) {
            if (args.files.length > 0) {
                vm.file = args.files[0];
                vm.fileName = vm.file.name;
            } else if (args.files.length <= 0 || vm.processing) {
                vm.file = null;
                return;
            }

            vm.noFile = false;

            var extension = vm.fileName.substring(vm.fileName.lastIndexOf(".") + 1, vm.fileName.length).toLowerCase();
            if (extension !== 'csv') {
                vm.invalidFileFormat = true;
                $timeout(function () {
                    vm.file = null;
                    vm.invalidFileFormat = false;
                }, 500);
                return;
            }
        });

        function activate() {
            vm.file = null;
            vm.fileName = '';
        }

        activate();
    }
    angular.module('umbraco').controller('24Days.ArticleImportController', ArticleImportController);
}());