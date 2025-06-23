#!/usr/bin/env sh

set -e

# Constants.
install_base="${XDG_DATA_HOME:-$HOME/.local/share}"
sourcegit_dir="$install_base"/sourcegit
applications_dir="$install_base"/applications

# Default values for configuration variables.
prevent_im_module=false
scale_factor=1

# Read configuration variables if file exists.
conf_file="$applications_dir"/SourceGit.desktop.conf
if test -f "$conf_file"
then
	# Reading a non-constant source can't be checked by shellcheck:
	# shellcheck disable=SC1090
	. "$conf_file"
fi

########################################
# Handle IM module.
#

if test "$prevent_im_module" = true
then
	export AVALONIA_IM_MODULE=none
fi

########################################
# Calculate scaling factors.
#

if which xrandr >/dev/null 2>&1
then
	scale_factors_spec=''

	active_monitor_specs=$(xrandr --listactivemonitors | tail -n +2 | cut -d ' ' -f 4,6 | sed 's/ /,/g')
	for monitor_spec in $active_monitor_specs
	do
		monitor=$(echo "$monitor_spec" | cut -d ',' -f 2)
		resolution=$(echo "$monitor_spec" | cut -d ',' -f 1 | cut -d '+' -f 1)
		dpmm_x=$(echo "$resolution" | cut -d 'x' -f 1)
		dpmm_y=$(echo "$resolution" | cut -d 'x' -f 2)
		resolution_x=$(echo "$dpmm_x" | cut -d '/' -f 1)
		resolution_y=$(echo "$dpmm_y" | cut -d '/' -f 1)
		size_x=$(echo "$dpmm_x" | cut -d '/' -f 2)
		size_y=$(echo "$dpmm_y" | cut -d '/' -f 2)

		factor=$(
			bc <<- EOF
				scale = 9;
				dpmm = ($resolution_x / $size_x + $resolution_y / $size_y) / 2
				dpi = dpmm * 25.4
				monitor_factor = dpi / 96
                factor = monitor_factor * $scale_factor

                scale = 1; factor = (factor * 2 + 0.05) / 1
                scale = 2; factor = (factor + 0.005) / 2

				factor
			EOF
		)

		if [ -z "$scale_factors_spec" ]
		then
			scale_factors_spec="$monitor=$factor"
		else
			scale_factors_spec="$scale_factors_spec;$monitor=$factor"
		fi
	done

	if [ -n "$scale_factors_spec" ]
	then
		export AVALONIA_SCREEN_SCALE_FACTORS="$scale_factors_spec"
	fi
fi

########################################
# Start SourceGit.
#

exec "$sourcegit_dir"/SourceGit
