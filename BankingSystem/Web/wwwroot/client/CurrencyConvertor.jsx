import * as React from "../lib/react";

class CommentBox extends React.Component {


    constructor() {
        super();
        console.log("From comment box");

    }

    render() {
        return(
            <div className="commentBox">
                React render

            </div>
        )
    }
}

ReactDOM.render(
    <CommentBox/>,
    document.getElementById("content"),
);
