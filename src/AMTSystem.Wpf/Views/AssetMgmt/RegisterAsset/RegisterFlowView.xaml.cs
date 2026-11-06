using System.Windows.Controls;

using AMTSystem.Core.Models;

namespace AMTSystem.Wpf.Views.AssetMgmt;
/// <summary>
/// Interaction logic for RegisterFlowView.xaml
/// </summary>
public partial class RegisterFlowView : UserControl
{
    private AssetDraft _draft = new AssetDraft();
    public event EventHandler Finished;

    public RegisterFlowView()
    {
        InitializeComponent();
        ShowForm();
    }

    public void ShowForm()
    {
        RegisterView form = new RegisterView(_draft);
        form.NextClicked += Form_NextClicked;
        StepArea.Content = form;
    }

    private void Form_NextClicked(object sender, EventArgs e)
    {
        ReviewFormView review = new ReviewFormView(_draft); //passes the contents of _draft from RegisterVIew into ReviewFormVIew
        // switches between the usercontrols of showing the register asset form and the submission action
        review.BackClicked += Review_BackClicked;
        review.SubmitClicked += Review_SubmitClicked;
        StepArea.Content = review;
    }

    private void Review_BackClicked(object sender, EventArgs e)
    {
        ShowForm();
    }

    private void Review_SubmitClicked(object sender, EventArgs e)
    {
        //DB code something
    }
}
