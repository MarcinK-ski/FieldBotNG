#!/bin/bash

# Exec example: ~/start-field-bot.sh FieldBotNG 1.0.1 &> /dev/null
# To kill, type: kill -9 `pgrep "FieldBotNG"`

APPNAME=$1
VERSION=$2
APPDIR=${APPNAME}_${VERSION}

STDOUTLOGS="logs.std.out"
STDERRLOGS="logs.std.err"

if ! pgrep -x "$APPNAME" > /dev/null
then
        echo "Starting ${APPNAME} - v${VERSION}!"
        cd ~/${APPDIR}

        touch $STDOUTLOGS
        touch $STDERRLOGS

        gzip -c $STDOUTLOGS >> ${STDOUTLOGS}.gz
        gzip -c $STDERRLOGS >> ${STDERRLOGS}.gz

        ( ./${APPNAME} > $STDOUTLOGS 2> $STDERRLOGS )&
else
        echo "App is already running!"
fi


