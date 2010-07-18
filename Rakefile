$: << "d:/documents/My Dropbox/code/ironbuildrake/lib/"

require 'build_engine'
require 'task_item'
require 'fileutils'

msbuild = tasks_from_module(Microsoft::Build::Tasks)

task :default => :build

task :build_portable do
  build_directory = 'src/Interface/bin/Debug/'
  output_directory = "build"
  mkdir_p output_directory

  files = %w{*.dll *.exe *.dll}
  cp_r(files.map {|f| Dir.glob(File.join(build_directory, f))}.flatten, output_directory)

  plugins_directory = File.join(output_directory,"Plugins")
  mkdir_p plugins_directory

  
  cp_r(Dir.glob(File.join(build_directory,"Plugins/*.dll")), plugins_directory)
  cp_r(File.join(build_directory, "Skins"), output_directory)
  cp_r(File.join(build_directory, "Utilities"), output_directory)
  
  system "junction #{output_directory}\\Plugins\\IronPythonPlugins #{build_directory}\\Plugins\\IronPythonPlugins"
end

task :build do
  msbuild.Message :text => "Building blaze"
  msbuild.MSBuild( { :projects => 'src\Automator.sln', :targets => "Build" } )

  msbuild.Message :text => "Build done"
end
