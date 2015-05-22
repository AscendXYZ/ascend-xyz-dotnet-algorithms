
module.exports = function (grunt) {

    grunt.loadNpmTasks('grunt-contrib-compress');

    grunt.initConfig({
        compress: {
            main: {
                options: {
                    archive: 'artifacts/Ascend.Algorithms.GdalInfo.zip'
                },
                files: [
                  {
                      expand: true,
                      cwd: 'artifacts/Ascend.Algorithms.GdalInfo/',
                      src: ['*'],
                      dest: ''
                  }, // includes files in path

                ]
            }
        }

    });
}