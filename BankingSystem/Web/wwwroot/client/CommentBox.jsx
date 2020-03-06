class CommentBox extends React.Component {
    constructor() {
        super();
        console.log("From comment box");

    }

    render() {
        return React.createElement(
            "div",
            { className: "commentBox" },
            "Hello, world! I am a CommentBox.",
        );
    }
}

ReactDOM.render(
    React.createElement(CommentBox, null),
    document.getElementById("content"),
);