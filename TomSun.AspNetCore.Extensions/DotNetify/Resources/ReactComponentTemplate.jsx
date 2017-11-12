var HelloWorld = React.createClass({
    getInitialState() {
        dotnetify.react.connect("[ComponentName]", this);
        return { Greetings: "", ServerTime: "" };
    },
    render() {
        return (
            [ComponentContent]
        );
    }
});

ReactDOM.render(
    <[ComponentName] />,
    document.getElementById('Content')
);