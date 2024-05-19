if [[ -z "$RIMWORLD_MOD_PATH" ]]; then
    RIMWORLD_MOD_PATH="../.export/"
    mkdir "../.export"
    echo "no RIMWORLD_MOD_PATH set exporing to ../.export"
else
    echo "exporing to $RIMWORLD_MOD_PATH"
fi

function recursiveCopyExt() {
    for filepath in $(find . -type f -name "*.$1");
    do
        cp --parents -f "$filepath" $RIMWORLD_MOD_PATH
    done
}

rm -rf "$RIMWORLD_MOD_PATH/*"
cp -rf "./About" "$RIMWORLD_MOD_PATH/About"
recursiveCopyExt "xml"
recursiveCopyExt "dll"
recursiveCopyExt "png"