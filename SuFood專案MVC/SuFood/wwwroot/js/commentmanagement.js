let app = new Vue({
    el: ".CommentReview",
    data:
    {
        keyword:"",
        topic: '評論管理',
        toast: "",
        deleteId: undefined,
        popupShowing: {
            showPopup: false
        },
        toastHintStyle: {
            fadeInUp: false
        },
        CreateOrEditOrDelete: "",
        or: [],
        createCommentList: {
            ReviewId: 0,
            RatingStar: 0,
            Comment: "",
            OrdersId: 0,
            AccountId: 0,
            Phone:"",
            Recomment:"",
        },
        editCommentList: {
            ReviewId: "",
            Comment: '',
            OrdersId: '',
        },
        modalContentStyle: {
            w200: true,
            w800: false
        },
        modalContainerStyle: {
            showModal: false,
        },
        isActive: false,
        selectedStars: 'all',
    },
    methods: {
        toastHint() {
            this.toastHintStyle.fadeInUp = true;
            setTimeout(() => {
            this.toastHintStyle.fadeInUp = false
            }, 2000)
        },
        createComment(c) {
            let _this = this;
            _this.createCommentList.ReviewId = c.reviewId;
            _this.createCommentList.RatingStar = c.ratingStar;
            _this.createCommentList.Comment = c.comment;
            _this.createCommentList.OrdersId = c.ordersId;
            _this.createCommentList.AccountId = c.accountId;
            _this.createCommentList.Phone = c.phone;
            _this.createCommentList.recomment = "";
            _this.CreateOrEditOrDelete = 'Create';
            _this.popupShowing.showPopup = true;
        },
        CloseComment() {
            this.popupShowing.showPopup = false;
        },
        closepopupShowHint() {
            this.popupShowing.showPopup = false;
            this.toastHint();
        },
        editComment(item) {
            this.CreateOrEditOrDelete = 'Edit';
            this.popupShowing.showPopup = true; 
            this.editCommentList = item;
        },
        deleteComment(c) {
            this.CreateOrEditOrDelete = 'Delete';
            this.popupShowing.showPopup = true;
            this.editCommentList = c;
        },
        openModal() {
            this.modalContainerStyle.showModal = true;
        },
        closeModal() {
            this.modalContainerStyle.showModal = false;
        },
        GetComments() {
            axios.get("/BackStage/CommentManagement/GetComment").then(response => {
                this.or = response.data
            })
        },
        DeleteComment: function (id) {
            var _this = this
            axios.delete(`/BackStage/CommentManagement/DeleteComment/${id}`).then(response => {
                this.toast = response.data;
                this.closepopupShowHint()
                _this.GetComments();
            })
        },
        CreateComment(createCommentList) {
            alert(createCommentList)
            let _this = this;
            axios.post('/BackStage/BackHome/CommentManagement/CreateComment', createCommentList).then(response => {
                this.toast = response.data;
                this.closepopupShowHint();
                this.GetComments();
            })
        },
        //沒用到
        EditComment: function (editCommentList) {
            var _this = this;
            var Cdata = {
                reviewId: _this.editCommentList.reviewId,
                ratingStar: _this.editCommentList.ratingStar,
                comment: _this.editCommentList.comment,
                ordersId: _this.editCommentList.ordersId
            };
            axios.post('/BackStage/CommentManagement/EditComment/', Cdata,{
                headers: {
                    'Content-Type':'application/json'
                }
            }).then(response => {
                this.toast = response.data,
                this.closepopupShowHint()
                _this.GetComments()
            })
        }
    },
    computed: {
        filterproducts() {
            let arr = this.or
            if (this.keyword != '') {
                arr = arr.filter(o => { return o.ordersId == this.keyword })
            }

            //下拉式選單篩選結果

            
            switch (this.selectedStars) {
                case "all":
                    break;
                case "good":
                    arr = arr.filter(o => {
                        return o.ratingStar == 5
                    })
                    break;
                case "bad":
                    arr = arr.filter(o => {
                        return o.ratingStar >= 1 && o.ratingStar < 5
                    })
                    break;
            }

            return arr
        }
    },
    mounted() {
        this.GetComments();
    }
})