/// <reference path="knockout-1.3.0beta.debug.js" />
/// <reference path="jquery-ui-1.8.16.js" />
/// <reference path="jquery-1.7.1.js" />
/// <reference path="ajax-util.js" />
/// <reference path="ko-protected-observable.js" />

$(document).ready(function () {
    $('#tagDialog').hide();
    var data = [
                { Id: 1, Name: "Ball Handling" },
                { Id: 2, Name: "Passing" },
                { Id: 3, Name: "Shooting" },
                { Id: 4, Name: "Rebounding" },
                { Id: 5, Name: "Transition" },
                { Id: 6, Name: "Defense" },
                { Id: 7, Name: "Team Offense" },
                { Id: 8, Name: "Team Defense" }
            ];

    //    var data = [
    //        new tagItem("Ball Handling", 1),
    //        new tagItem("Passing", 2),
    //        new tagItem("Shooting", 3),
    //        new tagItem("Rebounding", 4),
    //        new tagItem("Transition", 5),
    //        new tagItem("Defense", 6),
    //        new tagItem("Team Offense", 7),
    //        new tagItem("Team Defense", 8)
    //    ];

//    function tagItem(name, id) {
//        return {
//            Name: ko.protectedObservable(name),
//            Id: ko.protectedObservable(id)
//        }
//    }

    var viewModel = {
        tags: ko.observableArray(ko.toProtectedObservableItemArray(data)),
        tagToAdd: ko.observable(""),
        selectedTag: ko.observable(null),

        addTag: function () {
            this.tags.push({ Name: this.tagToAdd() });
            this.tagToAdd("")
        },

        selectTag: function () {

            viewModel.selectedTag(this);
        }

        //        addOnEnter: function (event) {
        //            var keycode = (event.which ? event.which : event.keycode);

        //            if (keycode === 13) {
        //                viewModel.addTag();
        //            }
        //            return true;
        //        }
    };

    $(document).on("click", ".tag-delete", function () {
        var itemToRemove = ko.dataFor(this);
        viewModel.tags.remove(itemToRemove);
    });

    $(document).on("click", ".tag-edit", function () {
        $('#tagDialog').dialog({
            buttons: {
                Save: function () {
                    $(this).dialog("close");
                    viewModel.selectedTag().Name.commit();
                },
                Cancel: function () { $(this).dialog("close") }
            }
        });
    });

    ko.applyBindings(viewModel);

//    function toTagItemArray(data) {
//        var tagItems = ko.utils.arrayMap(data, function (item) {
//            return new tagItem(item.Name, item.Id);
//        });
//        return tagItems;
//    }
});